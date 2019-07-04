using System.Threading.Tasks;
using Nodester.Common.Data;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Engine.Data;

namespace Bridge.Data
{
    public interface IBuildGraphService
    {

        Task ExecuteGraphAsync(User user, GraphDto graph);
        
        Task<IFlowGraph> BuildGraphAsync(User user, GraphDto graph);

    }
}