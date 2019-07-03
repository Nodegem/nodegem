using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bridge.Data;
using Mapster;
using Microsoft.Extensions.Logging;
using Nodester.Bridge.Extensions;
using Nodester.Common.Data;
using Nodester.Data.Dto.ComponentDtos;
using Nodester.Data.Dto.GraphDtos;
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
        private readonly INodesterUserService _userService;
        private readonly IServiceProvider _provider;
        private readonly IBuildMacroService _macroService;

        public GraphBuildService(ILogger<GraphBuildService> logger, INodesterUserService userService,
            IServiceProvider provider, IBuildMacroService macroService)
        {
            _logger = logger;
            _userService = userService;
            _provider = provider;
            _macroService = macroService;
        }

        public async Task<IFlowGraph> BuildGraph(GraphDto graph)
        {
            try
            {
                var userConstants = (await _userService.GetUserConstantsAsync()).ToList();
                var user = new User
                {
                    Email = AppState.Instance.Email,
                    Id = AppState.Instance.UserId.ToString(),
                    Username = AppState.Instance.Username,
                    Constants = userConstants
                        .ToDictionary(k => k.Key, c => c.Adapt<Constant>())
                };

                var constantDictionary = graph.Constants.Concat(userConstants)
                    .ToDictionary(k => k.Key, v => v.Adapt<Constant>());

                var nodes = await graph.Nodes.ToNodeDictionaryAsync(_provider, _macroService, user, constantDictionary);

                EstablishLinks(nodes, graph.Links);

                return new FlowGraph(nodes, constantDictionary, user);
            }
            // TODO: Make a graph build exception
            catch (GraphException ex)
            {
                _logger.LogError("Error within the graph.", ex);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong.", ex);
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