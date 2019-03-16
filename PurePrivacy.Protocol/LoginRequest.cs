using System;
using MessagePack;

namespace PurePrivacy.Protocol
{
    [MessagePackObject]
    public class LoginRequest
    {
        [Key(0)]
        public string UserName;
    }
}
