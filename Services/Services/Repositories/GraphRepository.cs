using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using Nodester.Common.Extensions;
using Nodester.Data.Contexts;
using Nodester.Data.Dto.ComponentDtos;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Data.Models;
using Nodester.Data.Models.Json_Models;
using Nodester.Data.Models.Json_Models.Graph_Constants;
using Nodester.Data.Settings;
using Nodester.Services.Data.Repositories;
using Twilio.TwiML.Voice;
using Start = Nodester.Graph.Core.Nodes.Essential.Start;

namespace Nodester.Services.Repositories
{
    public class GraphRepository : Repository<Nodester.Data.Models.Graph>, IGraphRepository
    {
        private readonly IDataProtector _protector;

        public GraphRepository(NodesterDBContext context, IDataProtectionProvider protectionProvider,
            IOptions<AppSettings> appSettings) : base(context)
        {
            _protector = protectionProvider.CreateProtector(appSettings.Value.SecretKey);
        }

        public async Task<GraphDto> GetGraphAsync(Guid graphId)
        {
            return UnprotectGraph(await GetAsync(graphId));
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
                    Id = Guid.NewGuid(), Position = Vector2.Default, FullName = Start.StartFullName, Permanent = true,
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
        
        private Nodester.Data.Models.Graph ProtectGraph(GraphDto graph)
        {
            var graphEntity = graph.Adapt<Nodester.Data.Models.Graph>();
            graphEntity.Constants = graphEntity.Constants.Select(x =>
            {
                var constant = x.Adapt<Constant>();
                constant.Value = constant.IsSecret ? _protector.Protect(constant.Value.ToString()) : constant.Value;
                return constant;
            });
            return graphEntity;
        }

        private GraphDto UnprotectGraph(Nodester.Data.Models.Graph graph)
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