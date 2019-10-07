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

        public static void AddOrUpdate<TSource>(this IList<TSource> source, TSource value, Func<TSource, bool> predicate = null)
        {
            if (predicate == null)
            {
                predicate = x => source.Equals(value);
            }

            var index = source.FindIndex(predicate);
            if (index > -1)
            {
                source[index] = value;
            }
            else
            {
                source.Add(value);
            }
        }

        public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate) {
            var retVal = 0;
            foreach (var item in items) {
                if (predicate(item)) return retVal;
                retVal++;
            }
            return -1;
        }

    }
}