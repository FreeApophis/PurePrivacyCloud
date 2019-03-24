using System.Threading.Tasks;
using PurePrivacy.Protocol;

namespace PurePrivacy.Server.MessageHandler
{
    public class NullHandler : MessageHandler
    {
        public override async Task HandleMessage(MessageHeader messageHeader, ConnectionInfo connectionInfo)
        {
        }
    }
}