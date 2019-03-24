using System.Collections.Generic;
using MessagePack;

namespace PurePrivacy.Protocol.Response
{
    [MessagePackObject]
    public class GetBlockResponse : IProtocolResponse
    {
        [IgnoreMember]
        public MessageType MessageType { get; } = MessageType.GetBlockResponse;

        [Key(0)]
        public List<byte> Block { get; set; }
    }
}