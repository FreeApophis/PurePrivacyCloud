using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace PurePrivacy.Server
{
    public class ConnectionInfo
    {
        public Socket Socket { get; }
        public NetworkStream Stream { get; }


        public bool IsServer { get; }

        public ConnectionInfo(Socket socket, bool isServer)
        {
            Socket = socket;
            IsServer = isServer;
            if (isServer == false)
            {
                Stream = new NetworkStream(socket, true);
            }
        }
    }
}