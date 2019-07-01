using Nodester.Graph.Core.Data.Fields;

namespace Nodester.Graph.Core.Data.Links
{
    public interface ILink<out TSource, out TDest> where TSource : IField where TDest : IField
    {
        TSource Source { get; }
        TDest Destination { get; }
    }
}