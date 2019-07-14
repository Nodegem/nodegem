using System.Threading.Tasks;
using Nodester.Common.Data;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Engine.Data;

namespace Bridge.Data
{
    public interface IBuildGraphService
    {

        Task ExecuteFlowGraphAsync(User user, GraphDto graph, bool isRunningLocally = true);
        
        Task<IFlowGraph> BuildGraphAsync(User user, GraphDto graph);
        
        Task<IListenerGraph> BuildListenerGraphAsync(User user, GraphDto graph);

    }
}