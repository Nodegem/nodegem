using System.Threading.Tasks;
using Nodegem.Common.Data;
using Nodegem.Engine.Core.Fields.Graph;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;

namespace Nodegem.Engine.Core.Nodes.Math
{
    [DefinedNode("DBC75648-B841-494A-8E56-565147DEEAD6")]
    [NodeNamespace("Core.Math")]
    public class Round : Node
    {
        [FieldAttributes(ValueType.Number)]
        public ValueInput Value { get; private set; }
        
        [FieldAttributes(ValueType.Number)]
        public ValueInput Digits { get; private set; }
        
        [FieldAttributes(ValueType.Number)]
        public ValueOutput Output { get; private set; }

        protected override void Define()
        {
            Value = AddValueInput<double>(nameof(Value));
            Digits = AddValueInput<int>(nameof(Digits));
            Output = AddValueOutput(nameof(Output), GetRoundedValue);
        }

        private async Task<double> GetRoundedValue(IFlow flow) =>
            System.Math.Round(await flow.GetValueAsync<double>(Value), await flow.GetValueAsync<int>(Digits));
    }
}