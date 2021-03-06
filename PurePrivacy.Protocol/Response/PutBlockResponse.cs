﻿using System.Collections.Generic;
using MessagePack;

namespace PurePrivacy.Protocol.Response
{
    [MessagePackObject]
    public class PutBlockResponse : IProtocolResponse
    {
        [IgnoreMember]
        public MessageType MessageType { get; } = MessageType.PutBlockResponse;

        [Key(0)]
        public List<byte> Key { get; set; }
    }
}