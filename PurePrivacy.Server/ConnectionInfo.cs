using System;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Logging;

namespace PurePrivacy.Server
{
    public class ConnectionInfo
    {
        public Socket Socket { get; }
        public NetworkStream Stream { get; }


        public bool IsServer { get; }
        public Guid ConnectionId { get; }

        public ConnectionInfo(Socket socket, bool isServer)
        {
            Socket = socket;
            IsServer = isServer;
            ConnectionId = Guid.NewGuid();
            if (isServer == false)
            {
                Stream = new NetworkStream(socket, true);
            }
        }
    }
}