using System;
using System.IO;
using System.Threading.Tasks;
using MsgPack.Serialization;
using Path = PurePrivacy.Core.Path;

namespace PurePrivacy.Client
{
    public class BasicClient : IClient
    {
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
    }
}