using System;
using System.Collections.Generic;
using System.Linq;

namespace Nodegem.Engine.Core.Extensions
{
    public static class LinqExtensions
    {
        public static bool TryGetValueOfType<TKey, TValue>(this Dictionary<TKey, TValue> source, Type type,
            out KeyValuePair<TKey, TValue> returnValue)
        {
            returnValue = source.FirstOrDefault(x => x.Value.GetType() == type);
            return !returnValue.Equals(null);
        }
    }
}