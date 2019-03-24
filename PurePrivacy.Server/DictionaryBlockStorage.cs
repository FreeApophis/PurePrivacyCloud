using System.Collections.Generic;
using Funcky.Extensions;

namespace PurePrivacy.Server
{
    public class DictionaryBlockStorage : IBlockStorage
    {
        private readonly Dictionary<List<byte>, List<byte>> _dictionary;

        public DictionaryBlockStorage()
        {
            _dictionary = new Dictionary<List<byte>, List<byte>>(new ListComparer<byte>());
        }

        public void PutBlock(List<byte> key, List<byte> block)
        {
            _dictionary[key] = block;
        }

        public List<byte> GetBlock(List<byte> key)
        {
            var option = _dictionary.TryGetValue(key);

            return option.Match(
                none: new List<byte>(),
                some: block => block
                );
        }
    }
}