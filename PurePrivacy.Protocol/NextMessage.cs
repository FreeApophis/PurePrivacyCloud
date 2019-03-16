using MessagePack;

namespace PurePrivacy.Protocol
{
    [MessagePackObject]
    public class NextMessage
    {
        [Key(0)]
        public MessageType MessageType;
    }
}
