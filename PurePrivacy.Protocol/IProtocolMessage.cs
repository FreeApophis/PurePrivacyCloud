namespace PurePrivacy.Protocol
{
    public interface IProtocolMessage
    {
        MessageType MessageType { get; }
        int MessageId { get; set; }
    }
}