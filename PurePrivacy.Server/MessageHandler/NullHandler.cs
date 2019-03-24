using System.Threading.Tasks;

namespace PurePrivacy.Server.MessageHandler
{
    public class NullHandler : MessageHandler
    {
        public override async Task HandleMessage(ConnectionInfo connectionInfo)
        {
        }
    }
}