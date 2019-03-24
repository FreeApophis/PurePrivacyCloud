using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using PurePrivacy.Protocol.Response;

namespace PurePrivacy.Client
{
    public interface IClient
    {
        Task Connect(IPAddress ipAddress, int port);
        Task<bool> Login(string userName);

        Task PutBlock(List<byte> key, List<byte> value);
        Task<List<byte>> GetBlock(List<byte> key);

        Task<StatusResponse> GetStatus();
    }
}
