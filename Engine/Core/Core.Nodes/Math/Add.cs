using System.Threading.Tasks;
using Nodegem.Common.Data;
using Nodegem.Engine.Core.Fields.Graph;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;

namespace Nodegem.Engine.Core.Nodes.Math
{
    [DefinedNode("31A4E6BC-EE2D-41D3-B0D2-24E014022B76")]
    [NodeNamespace("Core.Math")]
    public class Add : Node
    {
        [FieldAttributes(ValueType.Number)]
        public ValueInput A { get; private set; }

        [FieldAttributes(ValueType.Number)]
        public ValueInput B { get; private set; }

        [FieldAttributes("A + B", ValueType.Number)] 
        public ValueOutput Sum { get; private set; }

        protected override void Define()
        {
            A = AddValueInput<double>(nameof(A));
            B = AddValueInput<double>(nameof(B));
            Sum = AddValueOutput(nameof(Sum), GetSum);
        }

        private async Task<double> GetSum(IFlow flow)
        {
            return await flow.GetValueAsync<double>(A) + await flow.GetValueAsync<double>(B);
        }
    }
}