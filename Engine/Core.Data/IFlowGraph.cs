using Nodester.Engine.Data.Links;

namespace Nodester.Engine.Data
{
    public interface IFlowGraph : IGraph
    {
        void Run(bool isLocal = false);

        IFlowLink GetConnection(string fromFieldKey);
    }
}