using MessagePack;

namespace PurePrivacy.Protocol.Response
{
    [MessagePackObject]
    public class LoginResponse : IProtocolResponse
    {
        [IgnoreMember]
        public MessageType MessageType { get; } = MessageType.LoginResponse;

        [Key(0)]
        public bool Success { get; set; }
    }
}