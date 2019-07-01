using Nodester.Graph.Core.Data;
using Nodester.Graph.Core.Data.Attributes;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.Math
{
    [DefinedNode]
    [NodeNamespace("Core.Math")]
    public class Add : Node
    {
        [FieldAttributes(Type = ValueType.Number)]
        public ValueInput A { get; private set; }

        [FieldAttributes(Type = ValueType.Number)]
        public ValueInput B { get; private set; }

        [FieldAttributes("A + B")] 
        public ValueOutput Sum { get; private set; }

        protected override void Define()
        {
            A = AddValueInput<double>(nameof(A));
            B = AddValueInput<double>(nameof(B));
            Sum = AddValueOutput(nameof(Sum), GetSum);
        }

        private double GetSum(IFlow flow)
        {
            return flow.GetValue<double>(A) + flow.GetValue<double>(B);
        }
    }
}