using MessagePack;

namespace PurePrivacy.Protocol.Request
{
    [MessagePackObject]
    public class StatusRequest : IProtocolRequest
    {
        [IgnoreMember]
        public MessageType MessageType { get; } = MessageType.StatusRequest;

        [Key(0)]
        public int MessageId { get; set; }

        [Key(1)]
        public int Challenge { get; set; }
    }
}
