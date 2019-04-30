using System;

namespace PurePrivacy.CryptoFS
{
    public interface ICryptoFile
    {
        string Name { get; }
        DateTime CreationDateTime { get; }


        ISymmetricLink DataLink { get; }
        ISymmetricLink UpLink { get; }
        ISymmetricLink ClearanceLink { get; }

    }
}
