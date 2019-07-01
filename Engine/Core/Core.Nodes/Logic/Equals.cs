using Nodester.Graph.Core.Data;
using Nodester.Graph.Core.Data.Attributes;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.Logic
{
    [DefinedNode]
    [NodeNamespace("Core.Logic")]
    public class Equals : Node
    {
        public ValueInput A { get; private set; }
        public ValueInput B { get; private set; }

        [FieldAttributes("A = B")] public ValueOutput Result { get; private set; }

        protected override void Define()
        {
            A = AddValueInput<object>(nameof(A));
            B = AddValueInput<object>(nameof(B));
            Result = AddValueOutput<bool>(nameof(Result), Compare);
        }

        private bool Compare(IFlow flow)
        {
            return flow.GetValue<object>(A).Equals(flow.GetValue<object>(B));
        }
    }
}