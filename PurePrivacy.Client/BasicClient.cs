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

                if (_responses.ContainsKey(response.MessageId))
                {
                    _responses[response.MessageId].Add(response);
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
                    break;
                case MessageType.StatusRequest:
                    break;
                case MessageType.StatusResponse:
                    _logger.Log(LogLevel.Warning, "Deserialize Status Response");
                    return MessagePackSerializer.Deserialize<StatusResponse>(stream, true);
                case MessageType.LoginRequest:
                    break;
                case MessageType.LoginResponse:
                    _logger.Log(LogLevel.Warning, "Deserialize Login Response");
                    return MessagePackSerializer.Deserialize<LoginResponse>(stream, true);
                case MessageType.PutBlockRequest:
                    break;
                case MessageType.PutBlockResponse:
                    break;
                case MessageType.GetBlockRequest:
                    break;
                case MessageType.GetBlockResponse:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null);
            }

            return null;
        }

        public async Task<bool> Login(string userName)
        {
            var messageId = _messageId++;
            await SendNextMessageHeader(MessageType.LoginRequest);
            _responses[messageId] = new BlockingCollection<IProtocolResponse>();
            MessagePackSerializer.Serialize(_tcpClient.GetStream(), new LoginRequest { MessageId = messageId, UserName = userName });

            return WaitLoginReply(messageId);
        }

        private bool WaitLoginReply(int messageId)
        {
            return _responses[messageId].Take() is LoginResponse loginResponse && loginResponse.Success;
        }

        public async Task PutBlock(List<byte> key, List<byte> block)
        {
            await SendNextMessageHeader(MessageType.PutBlockRequest);
            await MessagePackSerializer.SerializeAsync(_tcpClient.GetStream(), new PutBlockRequest { Key = key, Block = block });

            await WaitPutBlockReply(key);

        }

        private async Task WaitPutBlockReply(List<byte> key)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<byte>> GetBlock(List<byte> key)
        {
            await SendNextMessageHeader(MessageType.GetBlockRequest);
            await MessagePackSerializer.SerializeAsync(_tcpClient.GetStream(), new GetBlockRequest { Key = key });

            return await WaitGetBlockReply(key);
        }

        public void Test()
        {
            throw new NotImplementedException();
        }

        public async Task<StatusResponse> GetStatus()
        {
            var challenge = _random.Next();
            var messageId = _messageId++;

            await SendNextMessageHeader(MessageType.StatusRequest);

            _responses[messageId] = new BlockingCollection<IProtocolResponse>();
            await MessagePackSerializer.SerializeAsync(_tcpClient.GetStream(), new StatusRequest { MessageId = messageId, Challenge = challenge });

            return WaitGetStatusReply(messageId);
        }

        private StatusResponse WaitGetStatusReply(int messageId)
        {
            return _responses[messageId].Take() as StatusResponse;
        }

        private async Task<List<byte>> WaitGetBlockReply(List<byte> key)
        {
            throw new System.NotImplementedException();
        }

        private async Task SendNextMessageHeader(MessageType messageType)
        {
            await MessagePackSerializer.SerializeAsync(_tcpClient.GetStream(),
                new MessageHeader { NextMessageType = messageType });
        }

    }
}