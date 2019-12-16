using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Data.Links
{
    public interface ILink<out TSource, out TDest> where TSource : IField where TDest : IField
    {
        TSource Source { get; }
        TDest Destination { get; }
    }
}