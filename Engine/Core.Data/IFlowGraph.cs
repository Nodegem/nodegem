using System.Threading.Tasks;
using Nodester.Engine.Data.Links;

namespace Nodester.Engine.Data
{
    public interface IFlowGraph : IGraph
    {
        Task RunAsync(bool isLocal = true);

        IFlowLink GetConnection(string fromFieldKey);
    }
}