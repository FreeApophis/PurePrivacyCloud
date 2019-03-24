using MessagePack;

namespace PurePrivacy.Protocol.Response
{
    [MessagePackObject]
    public class StatusResponse : IProtocolResponse
    {
        [IgnoreMember]
        public MessageType MessageType { get; } = MessageType.StatusResponse;

        [Key(0)]
        public int MessageId { get; set; }

        [Key(1)]
        public int Response { get; set; }

    }
}