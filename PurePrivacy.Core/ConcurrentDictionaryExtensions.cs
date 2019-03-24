using System.Collections.Concurrent;
using Funcky.Monads;

namespace PurePrivacy.Core
{
    public static class ConcurrentDictionaryExtensions
    {
        public static Option<TValue> TryRemove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.TryRemove(key, out TValue result)
                ? new Option<TValue>(result)
                : new Option<TValue>();
        }
    }
}
