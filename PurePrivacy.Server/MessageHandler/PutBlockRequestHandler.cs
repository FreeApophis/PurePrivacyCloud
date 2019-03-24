using System.Threading.Tasks;
using JetBrains.Annotations;
using MessagePack;
using PurePrivacy.Protocol;
using PurePrivacy.Protocol.Request;
using PurePrivacy.Protocol.Response;

namespace PurePrivacy.Server.MessageHandler
{
    public class PutBlockRequestHandler : MessageHandler
    {
        private readonly IBlockStorage _blockStorage;

        [UsedImplicitly]
        private const MessageType Type = MessageType.PutBlockRequest;

        public PutBlockRequestHandler(IBlockStorage blockStorage)
        {
            _blockStorage = blockStorage;
        }

        public override async Task HandleMessage(MessageHeader messageHeader, ConnectionInfo connectionInfo)
        {
            var putBlockRequest = MessagePackSerializer.Deserialize<PutBlockRequest>(connectionInfo.Stream, true);

            _blockStorage.PutBlock(putBlockRequest.Key, putBlockRequest.Block);

            await SendResponse<PutBlockResponse>(messageHeader, connectionInfo, new PutBlockResponse());
        }
    }
}