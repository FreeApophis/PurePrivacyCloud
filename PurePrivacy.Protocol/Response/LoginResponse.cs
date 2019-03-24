using MessagePack;

namespace PurePrivacy.Protocol.Response
{
    [MessagePackObject]
    public class LoginResponse : IProtocolResponse
    {
        [IgnoreMember]
        public MessageType MessageType { get; } = MessageType.LoginResponse;

        [Key(0)]
        public int MessageId { get; set; }

        [Key(1)]
        public bool Success { get; set; }
    }
}