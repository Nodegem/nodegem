using System.Threading.Tasks;
using Nodegem.Common.Data;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Logic
{
    [DefinedNode("1CED7167-9206-4D57-9B68-1DBBEF607D24", Title = "Greater Than Or Equal To")]
    [NodeNamespace("Core.Logic")]
    public class GreaterThanOrEqualTo : Node
    {
        
        public IFlowInputField In { get; set; }
        
        [FieldAttributes("A >= B")]
        public IFlowOutputField True { get; set; }
        
        [FieldAttributes("A < B")]
        public IFlowOutputField False { get; set; }
        
        
        [FieldAttributes(ValueType.Number)]
        public IValueInputField A { get; set; }
        
        [FieldAttributes(ValueType.Number)]
        public IValueInputField B { get; set; }
        
        protected override void Define()
        {
            In = AddFlowInput(nameof(In), IsGreaterThan);
            True = AddFlowOutput(nameof(True));
            False = AddFlowOutput(nameof(False));

            A = AddValueInput<object>(nameof(A), 0);
            B = AddValueInput<object>(nameof(B), 0);
        }

        private async Task<IFlowOutputField> IsGreaterThan(IFlow flow)
        {
            var a = await flow.GetValueAsync<double>(A);
            var b = await flow.GetValueAsync<double>(B);
            return a >= b ? True : False;
        }
    }
}