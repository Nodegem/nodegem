using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Nodester.Common.Data;
using Nodester.Common.Data.Interfaces;
using Nodester.Data.Dto.ComponentDtos;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Nodes;
using Nodester.Graph.Core;
using Nodester.Graph.Core.Fields.Graph;
using Nodester.Services.Data;
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

        public GraphManagerService(IServiceProvider serviceProvider, ITerminalHubService terminal,
            IMacroManagerService macroManager, IGraphRepository graphRepo)
        {
            _terminal = terminal;
            _provider = serviceProvider;
            _macroManager = macroManager;
            _graphRepo = graphRepo;
        }

        public async Task<IFlowGraph> BuildGraph(User user, RunGraphDto graph)
        {
            var graphConstants = await _graphRepo.GetConstantsAsync(graph.Id);
            var constantDictionary =
                graphConstants.ToDictionary(k => k.Key, x => x.Adapt<Constant>());

            await _terminal.DebugLogAsync(user, "Constructing nodes...", graph.IsDebugModeEnabled);
            var nodes = await graph.Nodes.ToNodeDictionaryAsync(_provider, _macroManager, user, constantDictionary);

            await _terminal.DebugLogAsync(user, "Establishing links between nodes...", graph.IsDebugModeEnabled);
            EstablishLinks(nodes, graph.Links);

            var newGraph = new FlowGraph(nodes, constantDictionary, user);
            await _terminal.DebugLogAsync(user, "Graph successfully built!", graph.IsDebugModeEnabled);

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