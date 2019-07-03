using System.Threading.Tasks;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Engine.Data;

namespace Bridge.Data
{
    public interface IBuildGraphService
    {

        Task<IFlowGraph> BuildGraph(GraphDto graph);

    }
}