using System.Threading.Tasks;
using MessagePack;
using PurePrivacy.Protocol;

namespace PurePrivacy.Server.MessageHandler
{
    public abstract class MessageHandler
    {
        public abstract Task HandleMessage(MessageHeader messageHeader, ConnectionInfo connectionInfo);

        protected async Task SendResponse<TResponse>(MessageHeader messageHeader, ConnectionInfo connectionInfo, IProtocolResponse response) where TResponse : class
        {
            await MessagePackSerializer.SerializeAsync(connectionInfo.Stream, new MessageHeader { MessageId = messageHeader.MessageId, NextMessageType = response.MessageType });
            await MessagePackSerializer.SerializeAsync<TResponse>(connectionInfo.Stream, response as TResponse);
            await connectionInfo.Stream.FlushAsync();
        }
    }
}