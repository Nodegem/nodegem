using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nodester.Common.Data;
using Nodester.Common.Extensions;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Fields;
using Nodester.Engine.Data.Nodes;
using Nodester.Graph.Core.Fields.Macro;

namespace Nodester.Graph.Core.Macro
{
    public class MacroGraph : BaseGraph, IMacroGraph
    {
        public IEnumerable<IMacroFlowInputField> FlowInputs { get; }
        public IEnumerable<IMacroFlowOutputField> FlowOutputs { get; }
        public IEnumerable<IMacroValueInputField> ValueInputs { get; }
        public IEnumerable<IMacroValueOutputField> ValueOutputs { get; }

        private IDictionary<string, IField> FieldDictionary { get; }

        private readonly IMacroFlow _flow;

        public MacroGraph(
            string name,
            Dictionary<Guid, INode> nodes,
            IEnumerable<IMacroFlowInputField> flowInputs, IEnumerable<IMacroFlowOutputField> flowOutputs,
            IEnumerable<IMacroValueInputField> valueInputs, IEnumerable<IMacroValueOutputField> valueOutputs,
            IDictionary<string, IField> fieldDictionary,
            User user)
            : base(name, nodes, user)
        {
            FieldDictionary = fieldDictionary;
            FlowInputs = flowInputs;
            FlowOutputs = flowOutputs;
            ValueInputs = valueInputs;
            ValueOutputs = valueOutputs;

            _flow = new MacroFlow(this);
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

        public async Task RunAsync(string flowInputFieldKey, bool isLocal = false)
        {
            await RunAsync(GetInputByKey(flowInputFieldKey), isLocal);
        }

        public async Task RunAsync(IMacroFlowInputField input, bool isLocal = false)
        {
            await _flow.RunAsync(input, isLocal);
        }

        public Task<T> GetValueAsync<T>(string key)
        {
            return GetValueAsync<T>(GetOutputByKey(key));
        }

        public Task<T> GetValueAsync<T>(IMacroValueOutputField output)
        {
            return _flow.GetValueAsync<T>(output);
        }

        public async Task<IFlowOutputField> ExecuteAsync(IMacroFlowInputField input, bool isLocal = true)
        {
            return await _flow.ExecuteAsync(input, isLocal);
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