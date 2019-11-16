using System.Threading.Tasks;
using Nodester.Common.Data;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.Math
{
    [DefinedNode]
    [NodeNamespace("Core.Math")]
    public class Logarithmic : Node
    {
        [FieldAttributes(ValueType.Number)]
        public IValueInputField A { get; set; }
        
        [FieldAttributes(ValueType.Number)]
        public IValueInputField B { get; set; }
        
        [FieldAttributes(ValueType.Number, Label = "LogB(A)")]
        public IValueOutputField Result  { get; set; }
        protected override void Define()
        {
            A = AddValueInput(nameof(A), default(double));
            B = AddValueInput<int>(nameof(B), 2);
            Result = AddValueOutput(nameof(Result), GetLog);
        }
        
        private async Task<double> GetLog(IFlow flow)
        {
            return System.Math.Log(await flow.GetValueAsync<double>(A), await flow.GetValueAsync<int>(B));
        }
    }
}