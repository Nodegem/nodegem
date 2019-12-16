using System.Threading.Tasks;
using Nodegem.Engine.Data.Links;

namespace Nodegem.Engine.Data
{
    public interface IFlowGraph : IGraph
    {
        Task RunAsync();

        IFlowLink GetConnection(string fromFieldKey);
    }
}