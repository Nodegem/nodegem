using System.Threading.Tasks;
using Nodegem.Common.Data;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Logic
{
    [DefinedNode("06DA8CEF-B37D-4978-AA02-40A36582E09B", Title = "Greater Than")]
    [NodeNamespace("Core.Logic")]
    public class GreaterThan : Node
    {
        
        public IFlowInputField In { get; set; }
        
        [FieldAttributes("A > B")]
        public IFlowOutputField True { get; set; }
        
        [FieldAttributes("A <= B")]
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
            return a > b ? True : False;
        }
    }
}