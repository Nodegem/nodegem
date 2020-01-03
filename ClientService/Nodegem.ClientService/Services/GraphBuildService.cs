using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nodegem.ClientService.Data;
using Nodegem.ClientService.Extensions;
using Nodegem.Common.Data;
using Nodegem.Common.Dto;
using Nodegem.Common.Dto.ComponentDtos;
using Nodegem.Engine.Core;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Exceptions;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Data.Nodes;

namespace Nodegem.ClientService.Services
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
                compiledGraph.IsRunningLocally = isRunningLocally;
                await compiledGraph.RunAsync();
            }
            catch (GraphBuildException ex)
            {
                _logger.LogError(ex, $"Error while building graph with ID: {graph.Id}");
                throw;
            }
            catch (FlowException ex)
            {
                _logger.LogError(ex, $"Error during graph execution with ID: {graph.Id}");
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

                var nodes = await graph.Nodes.ToNodeDictionaryAsync(graph.Links, provider.ServiceProvider, _macroService, user,
                    constantDictionary);

                EstablishLinks(nodes, graph.Links);

                return new FlowGraph(graph.Id, AppState.Instance.Info, graph.Name, nodes, constantDictionary, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error building graph with ID: {graph.Id}.");
                throw new GraphBuildException(ex.Message, null);
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

                var nodes = await graph.Nodes.ToNodeDictionaryAsync(graph.Links, provider.ServiceProvider, _macroService, user,
                    constantDictionary);

                EstablishLinks(nodes, graph.Links);

                return new ListenerGraph(graph.Id, AppState.Instance.Info, graph.Name, nodes, constantDictionary, user);
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