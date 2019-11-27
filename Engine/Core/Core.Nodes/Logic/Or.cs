using System.Threading.Tasks;
using Nodegem.Engine.Core.Fields.Graph;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;

namespace Nodegem.Engine.Core.Nodes.Logic
{
    [DefinedNode("FF36D925-EA48-4B58-A5DB-8D098C035292")]
    [NodeNamespace("Core.Logic")]
    public class Or : Node
    {
        public ValueInput A { get; private set; }
        public ValueInput B { get; private set; }

        [FieldAttributes("A | B")] public ValueOutput Result { get; private set; }

        protected override void Define()
        {
            A = AddValueInput<bool>(nameof(A));
            B = AddValueInput<bool>(nameof(B));
            Result = AddValueOutput(nameof(Result), PerformOrOp);
        }

        private async Task<bool> PerformOrOp(IFlow flow)
        {
            return await flow.GetValueAsync<bool>(A) || await flow.GetValueAsync<bool>(B);
        }
    }
}