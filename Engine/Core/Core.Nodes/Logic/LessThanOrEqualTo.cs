using System.Threading.Tasks;
using Nodegem.Common.Data;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Logic
{
    [DefinedNode("ED0BADC8-72C4-4A8C-91C0-485DF5F5540A", Title = "Less Than Or Equal To")]
    [NodeNamespace("Core.Logic")]
    public class LessThanOrEqualTo : Node
    {
        public IFlowInputField In { get; set; }
        
        [FieldAttributes("A <= B")]
        public IFlowOutputField True { get; set; }
        
        [FieldAttributes("A > B")]
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

            A = AddValueInput<double>(nameof(A));
            B = AddValueInput<double>(nameof(B));
        }

        private async Task<IFlowOutputField> IsGreaterThan(IFlow flow)
        {
            var a = await flow.GetValueAsync<double>(A);
            var b = await flow.GetValueAsync<double>(B);
            return a <= b ? True : False;
        }
    }
}