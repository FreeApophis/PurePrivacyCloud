using System;
using System.Threading.Tasks;
using MessagePack;
using PurePrivacy.Protocol;
using PurePrivacy.Protocol.Response;

namespace PurePrivacy.Server.MessageHandler
{
    public abstract class MessageHandler
    {
        public abstract Task HandleMessage(ConnectionInfo connectionInfo);

        protected async Task SendResponse<TResponse>(ConnectionInfo connectionInfo, IProtocolResponse response) where TResponse : class
        {
            await MessagePackSerializer.SerializeAsync(connectionInfo.Stream, new MessageHeader { NextMessageType = response.MessageType });
            await MessagePackSerializer.SerializeAsync<TResponse>(connectionInfo.Stream, response as TResponse);
            await connectionInfo.Stream.FlushAsync();
        }
    }
}