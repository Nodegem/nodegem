using Nodester.Engine.Data.Fields;

namespace Nodester.Engine.Data.Links
{
    public interface ILink<out TSource, out TDest> where TSource : IField where TDest : IField
    {
        TSource Source { get; }
        TDest Destination { get; }
    }
}