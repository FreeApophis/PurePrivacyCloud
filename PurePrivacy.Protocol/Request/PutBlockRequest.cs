using System.Collections.Generic;
using MessagePack;

namespace PurePrivacy.Protocol.Request
{
    [MessagePackObject]
    public class PutBlockRequest : IProtocolRequest
    {
        [IgnoreMember]
        public MessageType MessageType { get; } = MessageType.PutBlockRequest;

        [Key(0)]
        public List<byte> Key { get; set; }

        [Key(1)]
        public List<byte> Block { get; set; }
    }
}