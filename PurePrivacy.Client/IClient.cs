using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PurePrivacy.Client
{
    public interface IClient
    {
        Task CreateDirectory(Core.Path directoryPath);
        Task DeleteDirectory(Core.Path directoryPath);

        Task PutFile(Stream file, Core.Path filePath);
        Task DeleteFile(Core.Path filePath);
        Task<Stream> GetFile(Core.Path filePath);

    }
}
