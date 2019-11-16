using System.Threading.Tasks;
using Nodester.Common.Data;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.Math
{
    [DefinedNode]
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