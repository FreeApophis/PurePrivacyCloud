using System.Threading.Tasks;
using MessagePack;
using PurePrivacy.Protocol;

namespace PurePrivacy.Server.MessageHandler
{
    public abstract class MessageHandler
    {
        public abstract Task HandleMessage(ConnectionInfo connectionInfo);

        protected async Task SendResponse(ConnectionInfo connectionInfo, IProtocolResponse response)
        {
            await MessagePackSerializer.SerializeAsync(connectionInfo.Stream, new MessageHeader { NextMessageType = response.MessageType });
            await MessagePackSerializer.SerializeAsync(connectionInfo.Stream, response);
            await connectionInfo.Stream.FlushAsync();
        }
    }
}