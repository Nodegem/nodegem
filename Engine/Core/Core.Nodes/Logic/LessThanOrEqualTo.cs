using Nodester.Graph.Core.Data;
using Nodester.Graph.Core.Data.Attributes;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.Logic
{
    [DefinedNode("Less Than Or Equal To")]
    [NodeNamespace("Core.Logic")]
    public class LessThanOrEqualTo : Node
    {
        
        public FlowInput In { get; set; }
        
        [FieldAttributes("A <= B")]
        public FlowOutput True { get; set; }
        
        [FieldAttributes("A > B")]
        public FlowOutput False { get; set; }
        
        [FieldAttributes(Type = ValueType.Number)]
        public ValueInput A { get; set; }
        
        [FieldAttributes(Type = ValueType.Number)]
        public ValueInput B { get; set; }
        
        protected override void Define()
        {
            In = AddFlowInput(nameof(In), IsGreaterThan);
            True = AddFlowOutput(nameof(True));
            False = AddFlowOutput(nameof(False));

            A = AddValueInput<double>(nameof(A));
            B = AddValueInput<double>(nameof(B));
        }

        private FlowOutput IsGreaterThan(IFlow flow)
        {
            var a = flow.GetValue<double>(A);
            var b = flow.GetValue<double>(B);
            return a <= b ? True : False;
        }
    }
}