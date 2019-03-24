using MessagePack;

namespace PurePrivacy.Protocol.Request
{
    [MessagePackObject]
    public class LoginRequest : IProtocolRequest
    {
        [IgnoreMember]
        public MessageType MessageType { get; } = MessageType.LoginRequest;

        [Key(0)]
        public string UserName { get; set; }
    }
}
