using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MessagePack;
using Microsoft.Extensions.Logging;
using PurePrivacy.Core;
using PurePrivacy.Protocol;
using PurePrivacy.Protocol.Request;
using PurePrivacy.Protocol.Response;

namespace PurePrivacy.Client
{
    public class BasicClient : IClient
    {
        private readonly ILogger<BasicClient> _logger;
        private readonly TcpClient _tcpClient;
        private readonly Random _random;
        private int _messageId;

        private readonly ConcurrentDictionary<int, BlockingCollection<IProtocolResponse>> _responses = new ConcurrentDictionary<int, BlockingCollection<IProtocolResponse>>();

        public BasicClient(ILogger<BasicClient> logger)
        {
            _logger = logger;
            _tcpClient = new TcpClient();
            _random = new Random();
            _messageId = 42;
        }

        public async Task Connect(IPAddress ipAddress, int port)
        {
            await _tcpClient.ConnectAsync(ipAddress, port);

            new Thread(Listen) { Name = "Client Listener" }.Start();
        }

        private void Listen()
        {
            var stream = _tcpClient.GetStream();
            while (_tcpClient.Connected)
            {
                var nextMessage = MessagePackSerializer.Deserialize<MessageHeader>(stream, true);

                IProtocolResponse response = GetNextResponse(stream, nextMessage.NextMessageType);

                if (_responses.ContainsKey(nextMessage.MessageId))
                {
                    _responses[nextMessage.MessageId].Add(response);
                }
                else
                {
                    _logger.Log(LogLevel.Warning, "Message with Invalid Message ID...");
                }
            }
        }

        private IProtocolResponse GetNextResponse(Stream stream, MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.Invalid:
                    throw new ArgumentException(nameof(messageType));
                case MessageType.StatusRequest:
                    break;
                case MessageType.StatusResponse:
                    return MessagePackSerializer.Deserialize<StatusResponse>(stream, true);
                case MessageType.LoginRequest:
                    break;
                case MessageType.LoginResponse:
                    return MessagePackSerializer.Deserialize<LoginResponse>(stream, true);
                case MessageType.PutBlockRequest:
                    break;
                case MessageType.PutBlockResponse:
                    return MessagePackSerializer.Deserialize<PutBlockResponse>(stream, true);
                case MessageType.GetBlockRequest:
                    break;
                case MessageType.GetBlockResponse:
                    return MessagePackSerializer.Deserialize<GetBlockResponse>(stream, true);
                default:
                    throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null);
            }

            return null;
        }

        public async Task<bool> Login(string userName)
        {
            return await SendMessage(new LoginRequest { UserName = userName }, LoginReplyContinuation);
        }

        private bool LoginReplyContinuation(int messageId)
        {
            if (_responses[messageId].Take() is LoginResponse loginResponse)
            {
                RemoveContinuation(messageId);

                return loginResponse.Success;
            }

            throw new ArgumentException();
        }

        public async Task PutBlock(List<byte> key, List<byte> block)
        {
            await SendMessage(new PutBlockRequest { Key = key, Block = block }, PutBlockContinuation);
        }

        private Unit PutBlockContinuation(int messageId)
        {
            _responses[messageId].Take();

            RemoveContinuation(messageId);

            return new Unit();
        }

        public async Task<List<byte>> GetBlock(List<byte> key)
        {
            return await SendMessage(new GetBlockRequest { Key = key }, GetBlockContinuation);
        }

        private List<byte> GetBlockContinuation(int messageId)
        {

            if (_responses[messageId].Take() is GetBlockResponse getBlockResponse)
            {
                RemoveContinuation(messageId);

                return getBlockResponse.Block;
            }

            throw new ArgumentException();
        }

        private void RemoveContinuation(int messageId)
        {
            _responses.TryRemove(messageId);
        }

        public async Task<StatusResponse> GetStatus()
        {
            var challenge = _random.Next();

            return await SendMessage(new StatusRequest { Challenge = challenge }, StatusReplyContinuation);
        }

        private StatusResponse StatusReplyContinuation(int messageId)
        {
            if (_responses[messageId].Take() is StatusResponse statusResponse)
            {
                RemoveContinuation(messageId);

                return statusResponse;
            }
            throw new ArgumentException();
        }

        protected async Task<TWait> SendMessage<TResponse, TWait>(TResponse response, Func<int, TWait> continuation) where TResponse : class, IProtocolMessage
        {
            var messageId = NextMessageId();
            var stream = _tcpClient.GetStream();

            _responses[messageId] = new BlockingCollection<IProtocolResponse>();

            await MessagePackSerializer.SerializeAsync(stream, new MessageHeader { MessageId = messageId, NextMessageType = response.MessageType });
            await MessagePackSerializer.SerializeAsync(stream, response);

            return continuation(messageId);
        }

        private int NextMessageId() => _messageId++;

    }
}