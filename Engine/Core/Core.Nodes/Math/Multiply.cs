using System.Threading.Tasks;
using Nodegem.Common.Data;
using Nodegem.Engine.Core.Fields.Graph;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;

namespace Nodegem.Engine.Core.Nodes.Math
{
    [DefinedNode("EF801C31-4229-4116-A9CE-0482A444468B")]
    [NodeNamespace("Core.Math")]
    public class Multiply : Node
    {
        [FieldAttributes(ValueType.Number)]
        public ValueInput A { get; private set; }
        
        [FieldAttributes(ValueType.Number)]
        public ValueInput B { get; private set; }

        [FieldAttributes("A * B", ValueType.Number)] 
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