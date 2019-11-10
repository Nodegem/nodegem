using System.Threading.Tasks;
using Nodester.Common.Data;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.Math
{
    [DefinedNode]
    [NodeNamespace("Core.Math")]
    public class Add : Node
    {
        [FieldAttributes(ValueType.Number)]
        public ValueInput A { get; private set; }

        [FieldAttributes(ValueType.Number)]
        public ValueInput B { get; private set; }

        [FieldAttributes("A + B")] 
        public ValueOutput Sum { get; private set; }

        protected override void Define()
        {
            A = AddValueInput<double>(nameof(A));
            B = AddValueInput<double>(nameof(B));
            Sum = AddValueOutput(nameof(Sum), GetSum);
        }

        private async Task<double> GetSum(IFlow flow)
        {
            return await flow.GetValueAsync<double>(A) + await flow.GetValueAsync<double>(B);
        }
    }
}