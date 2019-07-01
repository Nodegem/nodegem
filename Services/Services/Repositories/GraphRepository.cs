using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nodester.Data.Contexts;
using Nodester.Data.Dto.ComponentDtos;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Data.Models.Json_Models;
using Nodester.Graph.Core.Essential;
using Nodester.Services.Data.Mappers;
using Nodester.Services.Data.Repositories;
using Node = Nodester.Data.Models.Json_Models.Node;

namespace Nodester.Services.Repositories
{
    public class GraphRepository : Repository<Nodester.Data.Models.Graph>, IGraphRepository
    {
        private IMapper<Nodester.Data.Models.Graph, CreateGraphDto> _createGraphMapper;
        private IMapper<Nodester.Data.Models.Graph, GraphDto> _graphMapper;

        public GraphRepository(NodesterDBContext context,
            IMapper<Nodester.Data.Models.Graph, CreateGraphDto> createGraphMapper,
            IMapper<Nodester.Data.Models.Graph, GraphDto> graphMapper) : base(context)
        {
            _graphMapper = graphMapper;
            _createGraphMapper = createGraphMapper;
        }

        public async Task<GraphDto> GetGraphAsync(Guid graphId)
        {
            return _graphMapper.ToDto(await GetAsync(graphId));
        }

        public IEnumerable<GraphDto> GetAllGraphsByUser(Guid userId)
        {
            return GetAll(x => x.UserId == userId).Select(g => _graphMapper.ToDto(g));
        }

        public async Task<IEnumerable<ConstantDto>> GetConstantsAsync(Guid graphId)
        {
            var graph = await GetGraphAsync(graphId);
            return graph?.Constants ?? new List<ConstantDto>();
        }

        public GraphDto CreateGraph(CreateGraphDto graph)
        {
            var newGraph = _createGraphMapper.ToModel(graph);
            Create(newGraph);
            return _graphMapper.ToDto(newGraph);
        }

        public GraphDto UpdateGraph(GraphDto graph)
        {
            var graphModel = _graphMapper.ToModel(graph);
            Update(graph.Id, graphModel);
            return _graphMapper.ToDto(graphModel);
        }

        public void DeleteGraph(Guid graphId)
        {
            Delete(graphId);
        }
    }
}