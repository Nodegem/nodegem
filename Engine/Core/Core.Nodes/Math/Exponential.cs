using System.Threading.Tasks;
using Nodester.Common.Data;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.Math
{
    [DefinedNode]
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