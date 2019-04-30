using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;

namespace PurePrivacy.CryptoFS
{
    public interface ICryptoFolder
    {
        string Name { get; }
        DateTime CreationDateTime { get; }

        IEnumerable<ICryptoFile> Files { get; }

        // Read Keys
        ISymmetricLink DataLink { get; }
        ISymmetricLink UpLink { get; }
        ISymmetricLink SubfolderLink { get; }
        ISymmetricLink FilesLink { get; }
        ISymmetricLink ClearanceLink { get; }

        // Write Keys
        IAsymmetricLink VerificationLink { get; }
        IAsymmetricLink SignatureLink { get; }
        IAsymmetricLink WriteSubfolderLink { get; }
        IAsymmetricLink WriteClearanceLink { get; }

    }
}
