using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace PurePrivacy.Server
{
    public class BlockServer : IBlockServer
    {
        public int Port { get; }
        public IPAddress IpAddress { get; set; } = IPAddress.Any;

        private readonly TcpListener _listener;
        public BlockServer(int port)
        {
            Port = port;
            _listener = new TcpListener(IpAddress, port);
        }

        public void Run()
        {
            _listener.Start();
            var task = AcceptClientsAsync(_listener);
            task.Wait();
        }

        async Task AcceptClientsAsync(TcpListener listener)
        {
            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();

                Debug.Assert(client.Connected);
            }
        }
    }

}
