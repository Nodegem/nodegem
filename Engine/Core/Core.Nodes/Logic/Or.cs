using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.Logic
{
    [DefinedNode]
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