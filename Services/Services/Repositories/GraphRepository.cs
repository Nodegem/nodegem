using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Nodester.Data.Contexts;
using Nodester.Data.Dto.ComponentDtos;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Data.Models;
using Nodester.Services.Data.Repositories;

namespace Nodester.Services.Repositories
{
    public class GraphRepository : Repository<Nodester.Data.Models.Graph>, IGraphRepository
    {
        public GraphRepository(NodesterDBContext context) : base(context)
        {
        }

        public async Task<GraphDto> GetGraphAsync(Guid graphId)
        {
            return (await GetAsync(graphId)).Adapt<GraphDto>();
        }

        public IEnumerable<GraphDto> GetGraphsAssignedToUser(Guid userId)
        {
            var graphs = GetAll(x => x.UserId == userId).OrderBy(x => x.CreatedOn);
            return graphs.Select(g => g.Adapt<GraphDto>());
        }

        public async Task<IEnumerable<ConstantDto>> GetConstantsAsync(Guid graphId)
        {
            var graph = await GetGraphAsync(graphId);
            return graph?.Constants ?? new List<ConstantDto>();
        }

        public GraphDto CreateGraph(CreateGraphDto graph)
        {
            var newGraph = graph.Adapt<Nodester.Data.Models.Graph>();
            Create(newGraph);
            return newGraph.Adapt<GraphDto>();
        }

        public GraphDto UpdateGraph(GraphDto graph)
        {
            if (graph.Type != ExecutionType.Recurring)
            {
                graph.RecurringOptions = null;
            }

            var graphModel = graph.Adapt<Nodester.Data.Models.Graph>();
            Update(graph.Id, graphModel);
            return graphModel.Adapt<GraphDto>();
        }

        public void DeleteGraph(Guid graphId)
        {
            Delete(graphId);
        }
    }
}