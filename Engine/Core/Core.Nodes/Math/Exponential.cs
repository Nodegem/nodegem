using System.Threading.Tasks;
using Nodegem.Common.Data;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Math
{
    [DefinedNode("DAEF96DA-78E1-4166-A6B7-220EE470A777")]
    [NodeNamespace("Core.Math")]
    public class Exponential : Node
    {
        
        [FieldAttributes(ValueType.Number)]
        public IValueInputField A { get; set; }
        
        [FieldAttributes(ValueType.Number)]
        public IValueInputField B { get; set; }
        
        [FieldAttributes(ValueType.Number, Label = "A ^ B")]
        public IValueOutputField Result  { get; set; }
        
        protected override void Define()
        {
            A = AddValueInput(nameof(A), default(double));
            B = AddValueInput(nameof(B), default(double));
            Result = AddValueOutput<double>(nameof(Result), GetExponential);
        }
        
        private async Task<double> GetExponential(IFlow flow)
        {
            return System.Math.Pow(await flow.GetValueAsync<double>(A), await flow.GetValueAsync<double>(B));
        }
    }
}