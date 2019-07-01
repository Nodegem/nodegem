using System;
using Nodester.Graph.Core.Data;
using Nodester.Graph.Core.Data.Attributes;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.Graph
{
    [DefinedNode("Get Variable")]
    [NodeNamespace("Core.Graph")]
    public class GetVariable : Node
    {
        public ValueInput Variable { get; private set; }
        public ValueOutput Value { get; private set; }

        protected override void Define()
        {
            Variable = AddValueInput<string>(nameof(Variable));
            Value = AddValueOutput(nameof(Value), GetValue);
        }

        private object GetValue(IFlow flow)
        {
            return Graph.GetVariable<object>(flow.GetValue<string>(Variable));
        }
    }
}