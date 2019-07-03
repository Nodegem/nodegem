using Nodester.Engine.Data.Links;

namespace Nodester.Engine.Data
{
    public interface IFlowGraph : IGraph
    {
        void Run();

        IFlowLink GetConnection(string fromFieldKey);
    }
}