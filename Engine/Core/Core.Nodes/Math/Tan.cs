using System.Threading.Tasks;
using Nodegem.Common.Data;
using Nodegem.Engine.Core.Fields.Graph;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;

namespace Nodegem.Engine.Core.Nodes.Math
{
    [DefinedNode("DD9D2C0C-1B30-4527-A0DA-46B5F237F77D")]
    [NodeNamespace("Core.Math")]
    public class Tan : Node
    {
        [FieldAttributes(ValueType.Number)]
        public ValueInput A { get; private set; }

        [FieldAttributes(ValueType.Number)]
        public ValueInput B { get; private set; }

        [FieldAttributes("A * tan(B)", ValueType.Number)] 
        public ValueOutput Result { get; private set; }

        protected override void Define()
        {
            A = AddValueInput<double>(nameof(A));
            B = AddValueInput<double>(nameof(B));
            Result = AddValueOutput(nameof(Result), GetResult);
        }

        private async Task<double> GetResult(IFlow flow)
        {
            var a = await flow.GetValueAsync<double>(A);
            var b = await flow.GetValueAsync<double>(B);
            return a * System.Math.Tan(b);
        }
    }
}