using System.Threading.Tasks;
using JetBrains.Annotations;
using MessagePack;
using Microsoft.Extensions.Logging;
using PurePrivacy.Protocol;
using PurePrivacy.Protocol.Request;
using PurePrivacy.Protocol.Response;

namespace PurePrivacy.Server.MessageHandler
{
    public class LoginRequestHandler : MessageHandler
    {
        [UsedImplicitly]
        private const MessageType Type = MessageType.LoginRequest;

        private readonly ILogger<LoginRequestHandler> _logger;

        public LoginRequestHandler(ILogger<LoginRequestHandler> logger)
        {
            _logger = logger;
        }


        public override async Task HandleMessage(ConnectionInfo connectionInfo)
        {
            var loginRequest = MessagePackSerializer.Deserialize<LoginRequest>(connectionInfo.Stream, true);

            _logger.Log(LogLevel.Information, $"Login Request with: {loginRequest.UserName}");

            await SendResponse<LoginResponse>(connectionInfo, new LoginResponse { MessageId = loginRequest.MessageId, Success = true });
        }
    }
}