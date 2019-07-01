using Nodester.Graph.Core.Data.Fields;
using Nodester.Graph.Core.Data.Links;

namespace Nodester.Graph.Core.Data
{
    public interface IFlowGraph : IGraph
    {
        void Run();

        IFlowLink GetConnection(string fromFieldKey);
    }
}