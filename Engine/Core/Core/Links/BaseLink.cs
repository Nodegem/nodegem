using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Data.Links;

namespace Nodegem.Engine.Core.Links
{
    public abstract class BaseLink<TSource, TDest> : ILink<TSource, TDest> where TSource : IField where TDest : IField
    {
        public TSource Source { get; private set; }
        public TDest Destination { get; private set; }

        protected BaseLink(TSource source, TDest dest)
        {
            Source = source;
            Destination = dest;
        }
    }
}