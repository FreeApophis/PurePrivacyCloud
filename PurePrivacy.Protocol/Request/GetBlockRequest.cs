using System.Collections.Generic;
using MessagePack;

namespace PurePrivacy.Protocol.Request
{
    [MessagePackObject]
    public class GetBlockRequest : IProtocolRequest
    {
        [IgnoreMember]
        public MessageType MessageType { get; } = MessageType.GetBlockRequest;

        [Key(0)]
        public List<byte> Key { get; set; }

    }
}