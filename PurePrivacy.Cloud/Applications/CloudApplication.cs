using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PurePrivacy.Client;
using PurePrivacy.Server;

namespace PurePrivacy.Cloud.Applications
{
    class CloudApplication : IApplication
    {
        private readonly IEnumerable<IBlockServer> _blockServers;
        private readonly IClient _client;

        public CloudApplication(Func<IBlockServer> createBlockServer, IClient client)
        {
            _client = client;

            _blockServers = Enumerable.Repeat(true, 8).Select(_ => createBlockServer());
        }

        public void Run(string[] args)
        {
            var threads = _blockServers.Select(blockServer => new Thread(blockServer.Run) { Name = $"Server on Port {blockServer.Port}" });

            foreach (var thread in threads)
            {
                thread.Start();
            }

            Thread.Sleep(2000);
            _client.Test();

            //using (FileStream stream = File.Open(@"c:\image.bmp", FileMode.Open))
            //{
            //    _client.PutFile(stream, new Path("image.bmp"));
            //}

            //_client.GetFile(new Path("image.bmp"));

            while (true) { Thread.Sleep(100); }
        }
    }
}
