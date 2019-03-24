using System;
using System.Collections.Generic;
using System.Linq;

namespace PurePrivacy.Server
{
    class ListComparer<TInner> : IEqualityComparer<List<TInner>>
    {
        public bool Equals(List<TInner> lhs, List<TInner> rhs)
        {
            if (lhs == null)
            {
                throw new ArgumentNullException(nameof(lhs));
            }

            if (rhs == null)
            {
                throw new ArgumentNullException(nameof(rhs));
            }

            return lhs.SequenceEqual(rhs);
        }

        public int GetHashCode(List<TInner> list)
        {
            return list.Aggregate(0, (hashcode, value) => hashcode ^ value.GetHashCode());
        }
    }
}