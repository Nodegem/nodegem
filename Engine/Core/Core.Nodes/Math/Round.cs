using Nodester.Graph.Core.Data;
using Nodester.Graph.Core.Data.Attributes;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.Math
{
    [DefinedNode]
    [NodeNamespace("Core.Math")]
    public class Round : Node
    {
        public ValueInput Value { get; private set; }
        public ValueInput Digits { get; private set; }
        public ValueOutput Output { get; private set; }

        protected override void Define()
        {
            Value = AddValueInput<double>(nameof(Value));
            Digits = AddValueInput<int>(nameof(Digits));
            Output = AddValueOutput(nameof(Output), GetRoundedValue);
        }

        private double GetRoundedValue(IFlow flow) =>
            System.Math.Round(flow.GetValue<double>(Value), flow.GetValue<int>(Digits));
    }
}