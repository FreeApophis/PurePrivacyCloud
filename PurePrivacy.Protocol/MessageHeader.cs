using MessagePack;

namespace PurePrivacy.Protocol
{
    [MessagePackObject]
    public class MessageHeader : IProtocolMessage
    {
        [IgnoreMember]
        public MessageType MessageType { get; } = MessageType.Invalid;

        [Key(0)]
        public int MessageId { get; set; }

        [Key(1)]
        public MessageType NextMessageType;
    }
}
