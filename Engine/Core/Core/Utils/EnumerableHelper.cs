using System.Linq;
using System.Collections.Generic;

namespace Nodester.Graph.Core.Utils
{
    public class EnumerableHelper
    {
        public static IEnumerable<T> Concat<T>(params IEnumerable<T>[] sequences)
        {
            return sequences.SelectMany(x => x);
        }
    }
}