using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bridge.Data;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nodester.Bridge.Extensions;
using Nodester.Common.Data;
using Nodester.Data.Dto.ComponentDtos;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Data.Models;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Exceptions;
using Nodester.Engine.Data.Fields;
using Nodester.Engine.Data.Nodes;
using Nodester.Graph.Core;

namespace Nodester.Bridge.Services
{
    public class GraphBuildService : IBuildGraphService
    {
        private readonly ILogger<GraphBuildService> _logger;
        private readonly IServiceProvider _provider;
        private readonly IBuildMacroService _macroService;

        public GraphBuildService(ILogger<GraphBuildService> logger,
            IServiceProvider provider, IBuildMacroService macroService)
        {
            _logger = logger;
            _provider = provider;
            _macroService = macroService;
        }

        public async Task ExecuteFlowGraphAsync(User user, GraphDto graph, bool isRunningLocally = true)
        {
            try
            {
                var compiledGraph = await BuildGraphAsync(user, graph);
                compiledGraph.DebugMode = graph.IsDebugModeEnabled;
                compiledGraph.IsRunningLocally = isRunningLocally;
                await compiledGraph.RunAsync();
            }
            catch (GraphException ex)
            {
                _logger.LogError(ex, $"Error during graph run with ID: {graph.Id}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong with graph ID: {graph.Id}");
                throw;
            }
        }

        public async Task<IFlowGraph> BuildGraphAsync(User user, GraphDto graph)
        {
            try
            {
                using var provider = _provider.CreateScope();
                var graphConstantDictionary = graph.Constants.ToDictionary(k => k.Key, v => v.Adapt<Constant>());
                var constantDictionary = graphConstantDictionary.Concat(user.Constants)
                    .ToDictionary(k => k.Key, v => v.Value.Adapt<Constant>());

                var nodes = await graph.Nodes.ToNodeDictionaryAsync(provider.ServiceProvider, _macroService, user,
                    constantDictionary);

                EstablishLinks(nodes, graph.Links);

                return new FlowGraph(graph.Id, graph.Name, nodes, constantDictionary, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error building graph with ID: {graph.Id}.");
                return null;
            }
        }

        public async Task<IListenerGraph> BuildListenerGraphAsync(User user, GraphDto graph)
        {
            try
            {
                using var provider = _provider.CreateScope();
                var graphConstantDictionary = graph.Constants.ToDictionary(k => k.Key, v => v.Adapt<Constant>());
                var constantDictionary = graphConstantDictionary.Concat(user.Constants)
                    .ToDictionary(k => k.Key, v => v.Value.Adapt<Constant>());

                var nodes = await graph.Nodes.ToNodeDictionaryAsync(provider.ServiceProvider, _macroService, user,
                    constantDictionary);

                EstablishLinks(nodes, graph.Links);

                return new ListenerGraph(graph.Id, graph.Name, nodes, constantDictionary, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error building listener graph with ID: {graph.Id}.");
                return null;
            }
        }

        private static void EstablishLinks(IDictionary<Guid, INode> nodes, IEnumerable<LinkDto> links)
        {
            foreach (var link in links)
            {
                var sourceNode = nodes[link.SourceNode];
                var destinationNode = nodes[link.DestinationNode];

                var sourceField = sourceNode.GetFieldByKey(link.SourceKey);
                var destinationField = destinationNode.GetFieldByKey(link.DestinationKey);

                if (sourceField is IFlowOutputField sourceFlow)
                {
                    sourceFlow.SetConnection((IFlowInputField) destinationField);
                }
                else if (destinationField is IValueInputField destinationInput)
                {
                    destinationInput.SetConnection((IValueOutputField) sourceField);
                }
            }
        }
    }
}