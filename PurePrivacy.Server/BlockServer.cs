using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using MessagePack;
using Microsoft.Extensions.Logging;
using PurePrivacy.Protocol;

namespace PurePrivacy.Server
{
    public class BlockServer : IBlockServer
    {
        private readonly ILogger _logger;
        private IPEndPoint _localEndPoint;
        private bool _connected;
        public int Port { get; }
        public IPAddress IpAddress { get; set; } = IPAddress.Any;
        public IDictionary<Socket, ConnectionInfo> Connections { get; } = new Dictionary<Socket, ConnectionInfo>();

        public BlockServer(ILogger logger, int port)
        {
            _logger = logger;
            Port = port;
        }

        public void Run()
        {
            _connected = true;

            var thread = new System.Threading.Thread(MainLoop) { Name = "Accept New Sockets" };

            thread.Start();
        }

        void MainLoop()
        {
            _localEndPoint = new IPEndPoint(IpAddress, Port);
            var connectSocket = new Socket(_localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Connections.Add(connectSocket, new ConnectionInfo(connectSocket, true));

            connectSocket.Bind(_localEndPoint);
            connectSocket.Listen(20);
            _logger.Log(LogLevel.Information, $"Listening on port: {Port}");

            while (_connected)
            {
                var activeSockets = new List<Socket>(Connections.Keys);
                var errorSockets = new List<Socket>();

                Socket.Select(activeSockets, null, errorSockets, 2000000);

                foreach (Socket socket in activeSockets)
                {
                    if (Connections[socket].IsServer)
                    {
                        HandleNewConnection(socket);
                    }
                    else
                    {
                        try
                        {
                            HandleIncomingMessage(Connections[socket]);
                        }
                        catch (InvalidOperationException)
                        {
                            HandleDisconnect(socket);
                        }

                    }
                }
            }

            GC.Collect();
        }

        private void HandleDisconnect(Socket socket)
        {
            _logger.Log(LogLevel.Information, Port, "Socket removed...");
            Connections.Remove(socket);
        }

        private void HandleIncomingMessage(ConnectionInfo connectionInfo)
        {
            _logger.Log(LogLevel.Information, Port, "Message received");
            var nextMessage = MessagePackSerializer.Deserialize<NextMessage>(connectionInfo.Stream, true);

            switch (nextMessage.MessageType)
            {
                case MessageType.DummyRequest:
                    var dummyRequest = MessagePackSerializer.Deserialize<DummyRequest>(connectionInfo.Stream, true);
                    _logger.Log(LogLevel.Information, Port, "Dummy Request");
                    break;
                case MessageType.LoginRequest:
                    var loginRequest = MessagePackSerializer.Deserialize<LoginRequest>(connectionInfo.Stream, true);
                    _logger.Log(LogLevel.Information, Port, $"Login Request with: {loginRequest.UserName}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(NextMessage));
            }
        }

        private void HandleNewConnection(Socket serverSocket)
        {
            _logger.Log(LogLevel.Information, Port, "New Connection");

            var clientSocket = serverSocket.Accept();
            Connections.Add(clientSocket, new ConnectionInfo(clientSocket, false));
        }
    }
}
