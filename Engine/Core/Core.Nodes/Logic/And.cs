using System.Threading.Tasks;
using Nodegem.Engine.Core.Fields.Graph;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;

namespace Nodegem.Engine.Core.Nodes.Logic
{
    [DefinedNode("25119992-7433-4E31-A39D-A6B57ABDFA37")]
    [NodeNamespace("Core.Logic")]
    public class And : Node
    {
        public ValueInput A { get; private set; }
        public ValueInput B { get; private set; }

        [FieldAttributes("A & B")] public ValueOutput Result { get; private set; }

        protected override void Define()
        {
            A = AddValueInput<bool>(nameof(A));
            B = AddValueInput<bool>(nameof(B));
            Result = AddValueOutput<bool>(nameof(Result), PerformAndOp);
        }

        private async Task<bool> PerformAndOp(IFlow flow)
        {
            return await flow.GetValueAsync<bool>(A) && await flow.GetValueAsync<bool>(B);
        }
    }
}