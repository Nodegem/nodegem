using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nodester.Common.Data;
using Nodester.Data.Dto.MacroDtos;
using Nodester.Graph.Core.Data;
using Nodester.Graph.Core.Data.Fields;
using Nodester.Graph.Core.Data.Nodes;
using Nodester.Graph.Core.Fields.Graph;
using Nodester.Graph.Core.Fields.Macro;
using Nodester.Graph.Core.Macro;
using Nodester.Graph.Core.Utils;
using Nodester.Services.Data;
using Nodester.Services.Data.Hubs;
using Nodester.Services.Data.Mappers;
using Nodester.Services.Data.Repositories;
using Nodester.Services.Extensions;

namespace Nodester.Services
{
    public class MacroManagerService : IMacroManagerService
    {
        private readonly IServiceProvider _provider;
        private readonly IMacroRepository _macroRepo;
        private readonly IMapper<MacroDto, RunMacroDto> _macroMapper;
        private readonly ITerminalHubService _terminal;

        public MacroManagerService(IServiceProvider serviceProvider,
            IMacroRepository macroRepo, IMapper<MacroDto, RunMacroDto> macroMapper, ITerminalHubService terminal)
        {
            _provider = serviceProvider;
            _macroRepo = macroRepo;
            _macroMapper = macroMapper;
            _terminal = terminal;
        }

        public async Task<IMacroGraph> BuildMacroAsync(User user, Guid id)
        {
            return await BuildMacroAsync(user, _macroMapper.ToDto(await _macroRepo.GetById(id)));
        }

        public async Task<IMacroGraph> BuildMacroAsync(User user, RunMacroDto macro)
        {
            await _terminal.SendDebugLogAsync(user, "Building nodes...", macro.IsDebugModeEnabled);
            var nodes = await macro.Nodes.ToNodeDictionaryAsync(_provider, this, user);
            
            await _terminal.SendDebugLogAsync(user, "Building fields...", macro.IsDebugModeEnabled);
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
            
            await _terminal.SendDebugLogAsync(user, "Establishing connections between fields...", macro.IsDebugModeEnabled);
            EstablishConnections(nodes, fieldDictionary, links);

            var builtMacro = new MacroGraph(nodes, fields.FlowInputs, fields.FlowOutputs,
                fields.ValueInputs, fields.ValueOutputs, fieldDictionary, user);

            await _terminal.SendDebugLogAsync(user, "Macro successfully built!", macro.IsDebugModeEnabled);
            return builtMacro;
        }

        private void EstablishConnections(IDictionary<Guid, INode> nodes, IDictionary<string, IField> fieldDictionary,
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
                    case MacroFlowInput sourceFlowInput:
                        sourceFlowInput.SetConnection((FlowInput) destination);
                        break;
                    case FlowOutput sourceFlowOutput:
                        sourceFlowOutput.SetConnection((FlowInput) destination);
                        break;
                    default:
                        switch (destination)
                        {
                            case MacroValueOutput destinationMacroOutput:
                                destinationMacroOutput.SetConnection((ValueOutput) source);
                                break;
                            case ValueInput destinationValueInput:
                                destinationValueInput.SetConnection((ValueOutput) source);
                                break;
                        }
                        break;
                }
            }
        }

        private FieldsContainer BuildFields(RunMacroDto macro)
        {
            return new FieldsContainer
            {
                FlowInputs = macro.FlowInputs.Select(x => new MacroFlowInput(x.Key.ToString())).ToList(),
                FlowOutputs = macro.FlowOutputs.Select(x => new MacroFlowOutput(x.Key.ToString())).ToList(),
                ValueInputs = macro.ValueInputs.Select(x => new MacroValueInput(x.Key.ToString(), x.DefaultValue,
                    typeof(object))).ToList(),
                ValueOutputs = macro.ValueOutputs.Select(x => new MacroValueOutput(x.Key.ToString(),
                    typeof(object))).ToList()
            };
        }

        private class FieldsContainer
        {
            public List<MacroFlowInput> FlowInputs { get; set; }
            public List<MacroFlowOutput> FlowOutputs { get; set; }
            public List<MacroValueInput> ValueInputs { get; set; }
            public List<MacroValueOutput> ValueOutputs { get; set; }

            public IDictionary<string, IField> FieldDictionary =>
                EnumerableHelper.Concat<IField>(FlowInputs, FlowOutputs, ValueInputs, ValueOutputs)
                    .ToDictionary(k => k.Key, v => v);
        }
        
    }
}