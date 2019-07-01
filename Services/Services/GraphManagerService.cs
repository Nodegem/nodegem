using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Nodester.Common.Data;
using Nodester.Data.Dto.ComponentDtos;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Graph.Core;
using Nodester.Graph.Core.Data;
using Nodester.Graph.Core.Data.Nodes;
using Nodester.Graph.Core.Fields.Graph;
using Nodester.Services.Data;
using Nodester.Services.Data.Hubs;
using Nodester.Services.Data.Repositories;
using Nodester.Services.Extensions;

namespace Nodester.Services
{
    public class GraphManagerService : IGraphManagerService
    {
        private readonly IGraphRepository _graphRepo;
        private readonly IMacroManagerService _macroManager;
        private readonly ITerminalHubService _terminal;
        private readonly IServiceProvider _provider;
        private readonly IUserService _userService;

        public GraphManagerService(IServiceProvider serviceProvider, ITerminalHubService terminal,
            IMacroManagerService macroManager, IGraphRepository graphRepo, IUserService userService)
        {
            _terminal = terminal;
            _provider = serviceProvider;
            _macroManager = macroManager;
            _graphRepo = graphRepo;
            _userService = userService;
        }

        public async Task<IFlowGraph> BuildGraph(User user, RunGraphDto graph)
        {
            var userConstants = await _userService.GetConstantsAsync(Guid.Parse(user.Id));
            var graphConstants = await _graphRepo.GetConstantsAsync(graph.Id);
            var combinedConstants = userConstants.Concat(graphConstants);
            var constantDictionary = combinedConstants.ToDictionary(k => k.Key, x => x.Adapt<Constant>());

            await _terminal.SendDebugLogAsync(user, "Constructing nodes...", graph.IsDebugModeEnabled);
            var nodes = await graph.Nodes.ToNodeDictionaryAsync(_provider, _macroManager, user, constantDictionary);

            await _terminal.SendDebugLogAsync(user, "Establishing links between nodes...", graph.IsDebugModeEnabled);
            EstablishLinks(nodes, graph.Links);
            
            var newGraph = new FlowGraph(nodes, constantDictionary, user);
            await _terminal.SendDebugLogAsync(user, "Graph successfully built!", graph.IsDebugModeEnabled);

            return newGraph;
        }

        private void EstablishLinks(Dictionary<Guid, INode> nodes, IEnumerable<LinkDto> links)
        {
            foreach (var link in links)
            {
                var sourceNode = nodes[link.SourceNode];
                var destinationNode = nodes[link.DestinationNode];

                var sourceField = sourceNode.GetFieldByKey(link.SourceKey);
                var destinationField = destinationNode.GetFieldByKey(link.DestinationKey);

                if (sourceField is FlowOutput sourceFlow)
                {
                    sourceFlow.SetConnection((FlowInput) destinationField);
                }
                else if (destinationField is ValueInput destinationInput)
                {
                    destinationInput.SetConnection((ValueOutput) sourceField);
                }
            }
        }

    }
}