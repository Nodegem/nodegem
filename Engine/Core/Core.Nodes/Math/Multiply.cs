using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
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

        private async Task<double> GetProduct(IFlow flow)
        {
            return await flow.GetValueAsync<double>(A) * await flow.GetValueAsync<double>(B);
        }
    }
}