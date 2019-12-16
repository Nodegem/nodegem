using System.Threading.Tasks;
using Nodegem.Common.Data;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Math
{
    [DefinedNode("B26345F9-992B-412E-A47E-F627F47B092E")]
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