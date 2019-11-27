using System.Collections.Generic;
using System.Linq;

namespace Nodegem.Engine.Core.Utils
{
    public class EnumerableHelper
    {
        public static IEnumerable<T> Concat<T>(params IEnumerable<T>[] sequences)
        {
            return sequences.SelectMany(x => x);
        }
    }
}