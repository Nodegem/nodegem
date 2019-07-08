using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.Logic
{
    [DefinedNode("Less Than")]
    [NodeNamespace("Core.Logic")]
    public class LessThan : Node
    {
        
        public IFlowInputField In { get; set; }
        
        [FieldAttributes("A < B")]
        public IFlowOutputField True { get; set; }
        
        [FieldAttributes("A >= B")]
        public IFlowOutputField False { get; set; }
        
        [FieldAttributes(Type = ValueType.Number)]
        public IValueInputField A { get; set; }
        
        [FieldAttributes(Type = ValueType.Number)]
        public IValueInputField B { get; set; }
        
        protected override void Define()
        {
            In = AddFlowInput(nameof(In), IsGreaterThan);
            True = AddFlowOutput(nameof(True));
            False = AddFlowOutput(nameof(False));

            A = AddValueInput<object>(nameof(A), 0);
            B = AddValueInput<object>(nameof(B), 0);
        }

        private Task<IFlowOutputField> IsGreaterThan(IFlow flow)
        {
            var a = flow.GetValue<double>(A);
            var b = flow.GetValue<double>(B);
            return Task.FromResult(a < b ? True : False);
        }
    }
}