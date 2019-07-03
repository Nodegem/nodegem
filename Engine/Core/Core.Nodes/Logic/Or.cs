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

        private bool PerformOrOp(IFlow flow)
        {
            return flow.GetValue<bool>(A) || flow.GetValue<bool>(B);
        }
    }
}