using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MessagePack;
using PurePrivacy.Protocol;
using PurePrivacy.Protocol.Request;
using PurePrivacy.Protocol.Response;

namespace PurePrivacy.Server.MessageHandler
{
    public class GetBlockRequestHandler : MessageHandler
    {
        private readonly IBlockStorage _blockStorage;
        [UsedImplicitly] private const MessageType Type = MessageType.GetBlockRequest;

        public GetBlockRequestHandler(IBlockStorage blockStorage)
        {
            _blockStorage = blockStorage;
        }

        public override async Task HandleMessage(MessageHeader messageHeader, ConnectionInfo connectionInfo)
        {
            var getBlockRequest = MessagePackSerializer.Deserialize<GetBlockRequest>(connectionInfo.Stream, true);
            var block = _blockStorage.GetBlock(getBlockRequest.Key);

            await SendResponse<GetBlockResponse>(messageHeader, connectionInfo, new GetBlockResponse { Block = block });
        }
    }
}