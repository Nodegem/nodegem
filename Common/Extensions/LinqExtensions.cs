using System;
using System.Collections;
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

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            var list = enumerable.ToList();
            for (var i = 0; i < list.Count; i++)
            {
                action(list[i], i);
            }
        }

        public static async Task<IEnumerable<TResult>> SelectAsync<TSource, TResult>(this IEnumerable<TSource> source,
            Func<TSource, Task<TResult>> method)
        {
            return await Task.WhenAll(source.Select(async s => await method(s)));
        }

        public static IEnumerable<TSource> Slice<TSource>(this IEnumerable<TSource> source, int start, int to)
        {
            return source.Skip(start).Take(to);
        }

    }
}