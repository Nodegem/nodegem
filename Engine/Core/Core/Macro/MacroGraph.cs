using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nodegem.Common;
using Nodegem.Common.Data;
using Nodegem.Common.Extensions;
using Nodegem.Engine.Core.Fields.Macro;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Data.Nodes;

namespace Nodegem.Engine.Core.Macro
{
    public class MacroGraph : BaseGraph, IMacroGraph
    {
        public IEnumerable<IMacroFlowInputField> FlowInputs { get; }
        public IEnumerable<IMacroFlowOutputField> FlowOutputs { get; }
        public IEnumerable<IMacroValueInputField> ValueInputs { get; }
        public IEnumerable<IMacroValueOutputField> ValueOutputs { get; }

        private IDictionary<string, IField> FieldDictionary { get; }

        private readonly IMacroFlow _macroFlow;

        public MacroGraph(
            Guid id,
            BridgeInfo bridge,
            string name,
            Dictionary<Guid, INode> nodes,
            IEnumerable<IMacroFlowInputField> flowInputs, IEnumerable<IMacroFlowOutputField> flowOutputs,
            IEnumerable<IMacroValueInputField> valueInputs, IEnumerable<IMacroValueOutputField> valueOutputs,
            IDictionary<string, IField> fieldDictionary,
            User user)
            : base(id, bridge, name, nodes, user)
        {
            FieldDictionary = fieldDictionary;
            FlowInputs = flowInputs;
            FlowOutputs = flowOutputs;
            ValueInputs = valueInputs;
            ValueOutputs = valueOutputs;

            _macroFlow = new MacroFlow(this);
            AssignMacroToNodes();
        }

        private void AssignMacroToNodes()
        {
            foreach (var node in Nodes)
            {
                node.Value.SetGraph(this);
            }
        }

        public IMacroFlowInputField GetInputByKey(string key)
        {
            return (IMacroFlowInputField) FieldDictionary[key];
        }

        public IMacroValueOutputField GetOutputByKey(string key)
        {
            return (IMacroValueOutputField) FieldDictionary[key];
        }

        public async Task RunAsync(string flowInputFieldKey)
        {
            await RunAsync(GetInputByKey(flowInputFieldKey));
        }

        public async Task RunAsync(IMacroFlowInputField input)
        {
            await _macroFlow.RunAsync(input);
        }

        public Task<T> GetValueAsync<T>(string key)
        {
            return GetValueAsync<T>(GetOutputByKey(key));
        }

        public Task<T> GetValueAsync<T>(IMacroValueOutputField output)
        {
            return _macroFlow.GetValueAsync<T>(output);
        }

        public async Task<IFlowOutputField> ExecuteAsync(IMacroFlowInputField input)
        {
            return await _macroFlow.ExecuteAsync(input);
        }

        public bool IsMacroFlowOutputField(string key)
        {
            if (!FieldDictionary.TryGetValue(key, out var field))
            {
                return false;
            }

            return field is MacroFlowOutput;
        }

        public void PopulateInputsWithNewDefaults(IEnumerable<FieldData> fieldData)
        {
            fieldData.ForEach(x => (FieldDictionary[x.Key] as MacroValueInput)?.SetValue(x.Value));
        }

        public INode ToMacroNode()
        {
            return new Nodes.Essential.Macro(this);
        }
    }
}