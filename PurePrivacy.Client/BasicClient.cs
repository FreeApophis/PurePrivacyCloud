using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using MessagePack;
using PurePrivacy.Protocol;
using Path = PurePrivacy.Core.Path;

namespace PurePrivacy.Client
{
    public class BasicClient : IClient
    {
        public BasicClient()
        {

        }

        public Task CreateDirectory(Path directoryPath)
        {
            throw new NotImplementedException();
        }

        public Task DeleteDirectory(Path directoryPath)
        {
            throw new System.NotImplementedException();
        }

        public Task PutFile(Stream file, Path filePath)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteFile(Path filePath)
        {
            throw new System.NotImplementedException();
        }

        public Task<Stream> GetFile(Path filePath)
        {
            throw new System.NotImplementedException();
        }

        public void Test()
        {
            var names = new List<string> { "Thomas", "Apophis", "Elian", "Xerxes" };
            using (TcpClient client = new TcpClient("localhost", 1982))
            {
                NetworkStream stream = client.GetStream();

                foreach (var name in names)
                {
                    MessagePackSerializer.Serialize(stream, new NextMessage { MessageType = MessageType.LoginRequest });
                    MessagePackSerializer.Serialize(stream, new LoginRequest { UserName = name });
                    stream.Flush();

                    System.Threading.Thread.Sleep(2000);
                }

                MessagePackSerializer.Serialize(stream, new NextMessage { MessageType = MessageType.DummyRequest });
                MessagePackSerializer.Serialize(stream, new DummyRequest());
                stream.Flush();

                System.Threading.Thread.Sleep(2000);

            }
        }
    }
}