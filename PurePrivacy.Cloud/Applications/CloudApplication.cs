using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using MsgPack.Serialization;
using PurePrivacy.Client;
using PurePrivacy.Server;
using Path = PurePrivacy.Core.Path;

namespace PurePrivacy.Cloud.Applications
{
    public enum MessageType { MessageA, MessageB, MessageC }

    public class MessageHeader
    {
        public MessageType MessageType { get; set; }
    }

    public class TestA
    {
        public string NothingToSee { get; set; }
    }

    public class TestB
    {
        public int Something { get; set; }
    }

    public class TestC
    {
        public string See { get; set; }
    }
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

            //using (FileStream stream = File.Open(@"c:\image.bmp", FileMode.Open))
            //{
            //    _client.PutFile(stream, new Path("image.bmp"));
            //}

            //_client.GetFile(new Path("image.bmp"));

            while (true) { Thread.Sleep(100); }
        }
    }
}
