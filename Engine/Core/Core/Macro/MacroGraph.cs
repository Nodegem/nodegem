using System;
using System.Collections.Generic;
using Nodester.Common.Data;
using Nodester.Common.Extensions;
using Nodester.Graph.Core.Data;
using Nodester.Graph.Core.Data.Fields;
using Nodester.Graph.Core.Data.Nodes;
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

        private readonly MacroFlow _flow;

        public MacroGraph(Dictionary<Guid, INode> nodes,
            IEnumerable<MacroFlowInput> flowInputs, IEnumerable<MacroFlowOutput> flowOutputs,
            IEnumerable<MacroValueInput> valueInputs, IEnumerable<MacroValueOutput> valueOutputs,
            IDictionary<string, IField> fieldDictionary,
            User user)
            : base(nodes, user)
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

        public void Run(string flowInputFieldKey)
        {
            Run(GetInputByKey(flowInputFieldKey));
        }

        public void Run(IMacroFlowInputField input)
        {
            _flow.Run(input);
        }

        public T GetValue<T>(string key)
        {
            return GetValue<T>(GetOutputByKey(key));
        }

        public T GetValue<T>(IMacroValueOutputField output)
        {
            return _flow.GetValue<T>(output);
        }

        public IFlowOutputField Execute(IMacroFlowInputField input)
        {
            return _flow.Execute(input);
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
            return new Essential.Macro(this);
        }
        
    }
}