using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nodester.Common.Extensions
{
    public static class LinqExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var value in enumerable)
            {
                action(value);
            }
        }

        public static IEnumerable<TSource> Slice<TSource>(this IEnumerable<TSource> source, int start, int to)
        {
            return source.Skip(start).Take(to);
        }
    }
}