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
using Nodegem.Engine.Core.Fields.Macro;
using Nodegem.Engine.Core.Macro;
using Nodegem.Engine.Core.Utils;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Exceptions;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Data.Nodes;

namespace Nodegem.ClientService.Services
{
    public class MacroBuildService : IBuildMacroService
    {
        private readonly ILogger<MacroBuildService> _logger;
        private readonly IServiceProvider _provider;
        private readonly INodegemApiService _apiService;

        public MacroBuildService(
            IServiceProvider serviceProvider,
            INodegemApiService apiService,
            ILogger<MacroBuildService> logger)
        {
            _provider = serviceProvider;
            _apiService = apiService;
            _logger = logger;
        }

        public async Task ExecuteMacroAsync(User user, MacroDto macro, string flowInputFieldId,
            bool isRunningLocally = true)
        {
            try
            {
                var compiledMacro = await BuildMacroAsync(user, macro);
                compiledMacro.IsRunningLocally = isRunningLocally;
                await compiledMacro.RunAsync(flowInputFieldId);
            }
            catch (GraphBuildException ex)
            {
                _logger.LogError(ex, $"Error while building macro with macro ID: {macro.Id}");
                throw;
            }
            catch (MacroFlowException ex)
            {
                _logger.LogError(ex, $"Error during macro excution with macro ID: {macro.Id}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong running macro with ID: {macro.Id}");
                throw;
            }
        }

        public async Task<IMacroGraph> BuildMacroAsync(User user, Guid id)
        {
            var macro = await _apiService.GraphService.GetMacroByIdAsync(id);

            // TODO: Throw a macro exception here
            if (macro == null)
            {
                throw new Exception("Macro could not be found or was corrupted");
            }

            return await BuildMacroAsync(user, macro);
        }

        public async Task<IMacroGraph> BuildMacroAsync(User user, MacroDto macro)
        {
            try
            {
                using var provider = _provider.CreateScope();
                var nodes = await macro.Nodes.ToNodeDictionaryAsync(macro.Links.Select(l => l.Adapt<LinkDto>()),
                    provider.ServiceProvider, this, user);

                if (!nodes.Any())
                {
                    throw new GraphBuildException("No nodes found", null);
                }

                var fields = BuildFields(macro);
                var fieldDictionary = fields.FieldDictionary;

                // Filter out links that are connected to field "nodes"
                var fieldNodes = macro.Nodes.Where(n => n.MacroFieldId.HasValue).ToList();
                var links = macro.Links.Select(x => new MacroLinkDto
                {
                    SourceNode = fieldNodes.Any(f => f.Id == x.SourceNode) ? null : x.SourceNode,
                    SourceKey = x.SourceKey,
                    DestinationNode = fieldNodes.Any(f => f.Id == x.DestinationNode) ? null : x.DestinationNode,
                    DestinationKey = x.DestinationKey
                });

                EstablishConnections(nodes, fieldDictionary, links);

                var builtMacro = new MacroGraph(macro.Id, AppState.Instance.Info, macro.Name, nodes, fields.FlowInputs,
                    fields.FlowOutputs,
                    fields.ValueInputs, fields.ValueOutputs, fieldDictionary, user);

                return builtMacro;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error building macro with ID: {macro.Id}");
                throw;
            }
        }

        private static void EstablishConnections(IDictionary<Guid, INode> nodes,
            IDictionary<string, IField> fieldDictionary,
            IEnumerable<MacroLinkDto> links)
        {
            foreach (var link in links)
            {
                IField source, destination;

                if (link.SourceNode.HasValue)
                {
                    var sourceNode = nodes[link.SourceNode.Value];
                    source = sourceNode.GetFieldByKey(link.SourceKey);
                }
                else
                {
                    source = fieldDictionary[link.SourceKey];
                }

                if (link.DestinationNode.HasValue)
                {
                    var destinationNode = nodes[link.DestinationNode.Value];
                    destination = destinationNode.GetFieldByKey(link.DestinationKey);
                }
                else
                {
                    destination = fieldDictionary[link.DestinationKey];
                }

                switch (source)
                {
                    case IMacroFlowInputField sourceFlowInput:
                        sourceFlowInput.SetConnection((IFlowInputField) destination);
                        break;
                    case IFlowOutputField sourceFlowOutput:
                        sourceFlowOutput.SetConnection((IFlowInputField) destination);
                        break;
                    default:
                        switch (destination)
                        {
                            case IMacroValueOutputField destinationMacroOutput:
                                destinationMacroOutput.SetConnection((IValueOutputField) source);
                                break;
                            case IValueInputField destinationValueInput:
                                destinationValueInput.SetConnection((IValueOutputField) source);
                                break;
                        }

                        break;
                }
            }
        }

        private static FieldsContainer BuildFields(MacroDto macro)
        {
            return new FieldsContainer
            {
                FlowInputs = macro.FlowInputs
                    .Select(x => (IMacroFlowInputField) new MacroFlowInput(x.Key.ToString())).ToList(),
                FlowOutputs = macro.FlowOutputs
                    .Select(x => (IMacroFlowOutputField) new MacroFlowOutput(x.Key.ToString())).ToList(),
                ValueInputs = macro.ValueInputs.Select(x => (IMacroValueInputField) new MacroValueInput(
                    x.Key.ToString(), x.DefaultValue,
                    typeof(object))).ToList(),
                ValueOutputs = macro.ValueOutputs.Select(x => (IMacroValueOutputField) new MacroValueOutput(
                    x.Key.ToString(),
                    typeof(object))).ToList()
            };
        }

        private class FieldsContainer
        {
            public IEnumerable<IMacroFlowInputField> FlowInputs { get; set; }
            public IEnumerable<IMacroFlowOutputField> FlowOutputs { get; set; }
            public IEnumerable<IMacroValueInputField> ValueInputs { get; set; }
            public IEnumerable<IMacroValueOutputField> ValueOutputs { get; set; }

            public IDictionary<string, IField> FieldDictionary =>
                EnumerableHelper.Concat<IField>(FlowInputs, FlowOutputs, ValueInputs, ValueOutputs)
                    .ToDictionary(k => k.Key, v => v);
        }
    }
}