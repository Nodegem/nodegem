using System.Threading.Tasks;
using Nodester.Common.Data;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Graph.Core.Data;

namespace Nodester.Services.Data
{
    public interface IGraphManagerService
    {
        Task<IFlowGraph> BuildGraph(User user, RunGraphDto graph);
    }
}