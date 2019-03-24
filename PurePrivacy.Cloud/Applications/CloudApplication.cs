using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using PurePrivacy.Client;
using PurePrivacy.Protocol.Response;
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

            ExecuteClientCommandsAsync().Wait();

            while (true) { Thread.Sleep(100); }
        }

        private async Task ExecuteClientCommandsAsync()
        {
            await _client.Connect(IPAddress.Parse("127.0.0.1"), 1982);

            bool success = await _client.Login("apophis@apophis.ch");
            Console.WriteLine($"Login  {(success ? "successful" : "unsuccessful")}");

            StatusResponse response = await _client.GetStatus();
            Console.WriteLine($"We got a response: {response.Response}");

            StatusResponse response2 = await _client.GetStatus();
            Console.WriteLine($"We got a response: {response2.Response}");

            List<byte> key = new List<byte> { 0xaf, 0xfe };
            List<byte> block = new List<byte> { 0xc0, 0xff, 0xee };

            await _client.PutBlock(key, block);
            var resultBlock = await _client.GetBlock(key);

            Debug.Assert(block.SequenceEqual(resultBlock));
            Console.WriteLine("Block stored and retrieved successfully");

        }
    }
}
