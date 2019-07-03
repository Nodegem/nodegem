using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.Math
{
    [DefinedNode]
    [NodeNamespace("Core.Math")]
    public class Subtract : Node
    {
        public ValueInput A { get; private set; }
        public ValueInput B { get; private set; }

        [FieldAttributes("A - B")] 
        public ValueOutput Difference { get; private set; }

        protected override void Define()
        {
            A = AddValueInput<int>(nameof(A));
            B = AddValueInput<int>(nameof(B));
            Difference = AddValueOutput(nameof(Difference), GetDifference);
        }

        private int GetDifference(IFlow flow)
        {
            return flow.GetValue<int>(A) - flow.GetValue<int>(B);
        }
    }
}