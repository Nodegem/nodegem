using System.Threading.Tasks;
using Nodegem.Common.Data;
using Nodegem.Common.Dto;
using Nodegem.Engine.Data;

namespace Nodegem.ClientService.Data
{
    public interface IBuildGraphService
    {

        Task ExecuteFlowGraphAsync(User user, GraphDto graph, bool isRunningLocally = true);
        
        Task<IFlowGraph> BuildGraphAsync(User user, GraphDto graph);
        
        Task<IListenerGraph> BuildListenerGraphAsync(User user, GraphDto graph);

    }
}