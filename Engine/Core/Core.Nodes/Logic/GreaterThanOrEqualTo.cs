using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.Logic
{
    [DefinedNode("Greater Than Or Equal To")]
    [NodeNamespace("Core.Logic")]
    public class GreaterThanOrEqualTo : Node
    {
        
        public FlowInput In { get; set; }
        
        [FieldAttributes("A >= B")]
        public FlowOutput True { get; set; }
        
        [FieldAttributes("A < B")]
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

            A = AddValueInput<object>(nameof(A), 0);
            B = AddValueInput<object>(nameof(B), 0);
        }

        private FlowOutput IsGreaterThan(IFlow flow)
        {
            var a = flow.GetValue<double>(A);
            var b = flow.GetValue<double>(B);
            return a >= b ? True : False;
        }
    }
}