namespace PurePrivacy.Protocol
{
    public enum MessageType
    {
        Invalid,

        StatusRequest,
        StatusResponse,
        LoginRequest,
        LoginResponse,
        PutBlockRequest,
        PutBlockResponse,
        GetBlockRequest,
        GetBlockResponse
    }
}