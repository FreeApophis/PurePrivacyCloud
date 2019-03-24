using Autofac.Features.Indexed;
using Funcky.Monads;

namespace PurePrivacy.Core
{
    public static class IndexExtensions
    {
        public static Option<TValue> TryGetValue<TKey, TValue>(this IIndex< TKey, TValue> index, TKey key)
        {
            return index.TryGetValue(key, out var result)
                ? new Option<TValue>(result)
                : new Option<TValue>();
        }
    }
}
