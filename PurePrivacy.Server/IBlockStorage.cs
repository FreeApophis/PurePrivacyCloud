using System.Collections.Generic;

namespace PurePrivacy.Server
{
    public interface IBlockStorage
    {
        void PutBlock(List<byte> key, List<byte> block);

        List<byte> GetBlock(List<byte> key);
    }
}
