using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bridge.Data;
using Microsoft.Extensions.Logging;
using Nodester.Bridge.Extensions;
using Nodester.Common.Data;
using Nodester.Common.Data.Interfaces;
using Nodester.Data.Dto.MacroDtos;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Fields;
using Nodester.Engine.Data.Nodes;
using Nodester.Graph.Core.Fields.Macro;
using Nodester.Graph.Core.Macro;
using Nodester.Graph.Core.Utils;

namespace Nodester.Bridge.Services
{
    public class MacroBuildService : IBuildMacroService
    {
        private readonly ILogger<MacroBuildService> _logger;
        private readonly IServiceProvider _provider;
        private readonly INodesterGraphService _graphService;
        private readonly ITerminalHubService _terminal;

        public MacroBuildService(IServiceProvider serviceProvider,
            INodesterGraphService graphService, ITerminalHubService terminal, ILogger<MacroBuildService> logger)
        {
            _provider = serviceProvider;
            _graphService = graphService;
            _terminal = terminal;
            _logger = logger;
        }

        public async Task ExecuteMacroAsync(User user, MacroDto macro)
        {
        }

        public async Task<IMacroGraph> BuildMacroAsync(User user, Guid id)
        {
            var macro = await _graphService.GetMacroByIdAsync(id);
            
            // TODO: Throw a macro exception here
            if (macro == null)
            {
                throw new Exception("Macro could not be found or was corrupted");
            }
            
            return await BuildMacroAsync(user, macro);
        }

        public async Task<IMacroGraph> BuildMacroAsync(User user, MacroDto macro)
        {
            var nodes = await macro.Nodes.ToNodeDictionaryAsync(_provider, this, user);

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

            var builtMacro = new MacroGraph(nodes, fields.FlowInputs, fields.FlowOutputs,
                fields.ValueInputs, fields.ValueOutputs, fieldDictionary, user);

            return builtMacro;
        }

        private void EstablishConnections(IDictionary<Guid, INode> nodes,
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