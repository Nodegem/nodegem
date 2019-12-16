using System.Threading.Tasks;
using Nodegem.Common.Data;
using Nodegem.Engine.Core.Fields.Graph;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;

namespace Nodegem.Engine.Core.Nodes.Math
{
    [DefinedNode("D4F8279A-E357-41C7-A18C-BE867C540D69")]
    [NodeNamespace("Core.Math")]
    public class Sin : Node
    {
        [FieldAttributes(ValueType.Number)]
        public ValueInput A { get; private set; }

        [FieldAttributes(ValueType.Number)]
        public ValueInput B { get; private set; }

        [FieldAttributes("A * sin(B)", ValueType.Number)] 
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
            return a * System.Math.Sin(b);
        }
    }
}