using System.Threading.Tasks;
using Nodegem.Common.Data;
using Nodegem.Engine.Core.Fields.Graph;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;

namespace Nodegem.Engine.Core.Nodes.Math
{
    [DefinedNode("B882CAC4-0831-4DF2-BFB4-A33352317F40")]
    [NodeNamespace("Core.Math")]
    public class Divide : Node
    {
        [FieldAttributes(ValueType.Number)]
        public ValueInput A { get; private set; }
        
        [FieldAttributes(ValueType.Number)]
        public ValueInput B { get; private set; }

        [FieldAttributes("A / B", ValueType.Number)] 
        public ValueOutput Dividend { get; private set; }

        protected override void Define()
        {
            A = AddValueInput<double>(nameof(A));
            B = AddValueInput<double>(nameof(B));
            Dividend = AddValueOutput(nameof(Dividend), GetDividend);
        }

        private async Task<double> GetDividend(IFlow flow)
        {
            return await flow.GetValueAsync<double>(A) / await flow.GetValueAsync<double>(B);
        }
    }
}