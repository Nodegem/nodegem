using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using Nodegem.Common.Data;
using Nodegem.Common.Dto;
using Nodegem.Data.Contexts;
using Nodegem.Data.Dto.GraphDtos;
using Nodegem.Data.Models;
using Nodegem.Data.Models.Json_Models;
using Nodegem.Data.Settings;
using Nodegem.Services.Data.Repositories;
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
            var graph = await GetAsync(graphId);
            var graphDto = graph.Adapt<GraphDto>();
            graphDto.Constants = UnProtectConstants(graphDto.Constants);
            return graphDto;
        }

        public async Task<bool> IsListenerGraphAsync(Guid graphId)
        {
            var graph = await GetGraphAsync(graphId);
            return graph.Type == ExecutionType.Listener;
        }

        public IEnumerable<GraphDto> GetGraphsAssignedToUser(Guid userId)
        {
            var graphs = GetAll(x => x.UserId == userId).OrderBy(x => x.CreatedOn);
            return graphs.Select(g =>
            {
                var graphDto = g.Adapt<GraphDto>();
                graphDto.Constants = UnProtectConstants(g.Constants);
                return graphDto;
            });
        }

        public async Task<IEnumerable<Constant>> GetConstantsAsync(Guid graphId)
        {
            var graph = await GetGraphAsync(graphId);
            return graph?.Constants ?? new List<Constant>();
        }

        public GraphDto CreateGraph(CreateGraphDto graph)
        {
            graph.Constants = ProtectConstants(graph.Constants);
            var newGraph = graph.Adapt<Graph>();
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

            var newGraphDto = newGraph.Adapt<GraphDto>();
            newGraphDto.Constants = UnProtectConstants(newGraph.Constants);
            return newGraphDto;
        }

        public GraphDto UpdateGraph(GraphDto graph)
        {
            if (graph.Type != ExecutionType.Recurring)
            {
                graph.RecurringOptions = null;
            }

            graph.Constants = ProtectConstants(graph.Constants);
            var graphEntity = graph.Adapt<Graph>();
            Update(graph.Id, graphEntity);

            var graphDto = graphEntity.Adapt<GraphDto>();
            graphDto.Constants = UnProtectConstants(graphDto.Constants);
            return graphDto;
        }

        public void DeleteGraph(Guid graphId)
        {
            Delete(graphId);
        }
        
        private IEnumerable<Constant> ProtectConstants(IEnumerable<Constant> constants)
        {
            return constants.Select(constant =>
            {
                constant.Value = constant.IsSecret
                    ? _protector.Protect(constant.Value.ToString())
                    : constant.Value;
                return constant;
            }).ToList();
        }
        
        private IEnumerable<Constant> UnProtectConstants(IEnumerable<Constant> constants)
        {
            return constants.Select(constant =>
            {
                constant.Value = constant.IsSecret
                    ? _protector.Unprotect(constant.Value.ToString())
                    : constant.Value;
                return constant;
            }).ToList();
        }
    }
}