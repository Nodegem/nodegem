using System;
using System.Collections.Generic;
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

        public void Run(string flowInputFieldKey, bool isLocal = false)
        {
            Run(GetInputByKey(flowInputFieldKey), isLocal);
        }

        public void Run(IMacroFlowInputField input, bool isLocal = false)
        {
            _flow.Run(input, isLocal);
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