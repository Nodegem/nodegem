using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using Nodegem.Common.Data;
using Nodegem.Common.Dto;
using Nodegem.Common.Dto.ComponentDtos;
using Nodegem.Data.Contexts;
using Nodegem.Data.Dto.GraphDtos;
using Nodegem.Data.Models;
using Nodegem.Data.Models.Json_Models;
using Nodegem.Data.Settings;
using Nodegem.Services.Data.Repositories;
using Constant = Nodegem.Data.Models.Json_Models.Graph_Constants.Constant;
using Start = Nodegem.Engine.Core.Nodes.Essential.Start;

namespace Nodegem.Services.Repositories
{
    public class GraphRepository : Repository<Graph>, IGraphRepository
    {
        private readonly IDataProtector _protector;

        public GraphRepository(NodegemContext context, IDataProtectionProvider protectionProvider,
            IOptions<AppSettings> appSettings) : base(context)
        {
            _protector = protectionProvider.CreateProtector(appSettings.Value.SecretKey);
        }

        public async Task<GraphDto> GetGraphAsync(Guid graphId)
        {
            return UnprotectGraph(await GetAsync(graphId));
        }

        public async Task<bool> IsListenerGraphAsync(Guid graphId)
        {
            var graph = await GetGraphAsync(graphId);
            return graph.Type == ExecutionType.Listener;
        }

        public IEnumerable<GraphDto> GetGraphsAssignedToUser(Guid userId)
        {
            var graphs = GetAll(x => x.UserId == userId).OrderBy(x => x.CreatedOn);
            return graphs.Select(UnprotectGraph);
        }

        public async Task<IEnumerable<ConstantDto>> GetConstantsAsync(Guid graphId)
        {
            var graph = await GetGraphAsync(graphId);
            return graph?.Constants ?? new List<ConstantDto>();
        }

        public GraphDto CreateGraph(CreateGraphDto graph)
        {
            var newGraph = ProtectGraph(graph.Adapt<GraphDto>());
            newGraph.IsActive = true;
            newGraph.Nodes = new List<Node>
            {
                new Node
                {
                    Id = Guid.NewGuid(), Position = Vector2.Default, DefinitionId = Start.StartDefinitionId,
                    Permanent = true,
                }
            };
            Create(newGraph);
            return UnprotectGraph(newGraph);
        }

        public GraphDto UpdateGraph(GraphDto graph)
        {
            if (graph.Type != ExecutionType.Recurring)
            {
                graph.RecurringOptions = null;
            }

            var graphEntity = ProtectGraph(graph);
            Update(graph.Id, graphEntity);
            return UnprotectGraph(graphEntity);
        }

        public void DeleteGraph(Guid graphId)
        {
            Delete(graphId);
        }

        private Graph ProtectGraph(GraphDto graph)
        {
            var graphEntity = graph.Adapt<Graph>();
            graphEntity.Constants = graphEntity.Constants.Select(x =>
            {
                var constant = x.Adapt<Constant>();
                constant.Value = constant.IsSecret ? _protector.Protect(constant.Value.ToString()) : constant.Value;
                return constant;
            });
            return graphEntity;
        }

        private GraphDto UnprotectGraph(Graph graph)
        {
            var graphDto = graph.Adapt<GraphDto>();
            graphDto.Constants = graphDto.Constants.Select(x =>
            {
                var constant = x.Adapt<ConstantDto>();
                constant.Value = constant.IsSecret ? _protector.Unprotect(constant.Value.ToString()) : constant.Value;
                return constant;
            });
            return graphDto;
        }
    }
}