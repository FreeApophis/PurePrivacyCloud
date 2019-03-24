using System.Threading.Tasks;
using JetBrains.Annotations;
using MessagePack;
using Microsoft.Extensions.Logging;
using PurePrivacy.Protocol;
using PurePrivacy.Protocol.Request;
using PurePrivacy.Protocol.Response;

namespace PurePrivacy.Server.MessageHandler
{
    class StatusRequestHandler : MessageHandler
    {
        [UsedImplicitly]
        private const MessageType Type = MessageType.StatusRequest;

        private readonly ILogger<StatusRequestHandler> _logger;

        public StatusRequestHandler(ILogger<StatusRequestHandler> logger)
        {
            _logger = logger;
        }


        public override async Task HandleMessage(ConnectionInfo connectionInfo)
        {
            _logger.Log(LogLevel.Information, "Handle Status Request");

            var statusRequest = MessagePackSerializer.Deserialize<StatusRequest>(connectionInfo.Stream, true);

            await SendResponse(connectionInfo, new StatusResponse { MessageId = statusRequest.MessageId, Response = 1337 });
        }
    }
}
