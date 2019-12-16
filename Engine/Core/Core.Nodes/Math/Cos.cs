using System.Threading.Tasks;
using Nodegem.Common.Data;
using Nodegem.Engine.Core.Fields.Graph;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;

namespace Nodegem.Engine.Core.Nodes.Math
{
    [DefinedNode("0651646D-2DBC-4A60-98D2-7D64197653F3")]
    [NodeNamespace("Core.Math")]
    public class Cos : Node
    {
        [FieldAttributes(ValueType.Number)]
        public ValueInput A { get; private set; }

        [FieldAttributes(ValueType.Number)]
        public ValueInput B { get; private set; }

        [FieldAttributes("A * cos(B)", ValueType.Number)] 
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
            return a * System.Math.Cos(b);
        }
    }
}