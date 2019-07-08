using System.Threading.Tasks;
using Nodester.Engine.Data.Links;

namespace Nodester.Engine.Data
{
    public interface IFlowGraph : IGraph
    {
        Task RunAsync(bool isLocal = false);

        IFlowLink GetConnection(string fromFieldKey);
    }
}