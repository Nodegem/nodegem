using Nodester.Graph.Core.Data;
using Nodester.Graph.Core.Data.Attributes;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.Math
{
    [DefinedNode]
    [NodeNamespace("Core.Math")]
    public class Multiply : Node
    {
        public ValueInput A { get; private set; }
        public ValueInput B { get; private set; }

        [FieldAttributes("A * B")] 
        public ValueOutput Product { get; private set; }

        protected override void Define()
        {
            A = AddValueInput<double>(nameof(A));
            B = AddValueInput<double>(nameof(B));
            Product = AddValueOutput(nameof(Product), GetProduct);
        }

        private double GetProduct(IFlow flow)
        {
            return flow.GetValue<double>(A) * flow.GetValue<double>(B);
        }
    }
}