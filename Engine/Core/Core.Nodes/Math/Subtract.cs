using System.Threading.Tasks;
using Nodester.Common.Data;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.Math
{
    [DefinedNode]
    [NodeNamespace("Core.Math")]
    public class Subtract : Node
    {
        [FieldAttributes(ValueType.Number)]
        public ValueInput A { get; private set; }
        
        [FieldAttributes(ValueType.Number)]
        public ValueInput B { get; private set; }

        [FieldAttributes("A - B", ValueType.Number)] 
        public ValueOutput Difference { get; private set; }

        protected override void Define()
        {
            A = AddValueInput<int>(nameof(A));
            B = AddValueInput<int>(nameof(B));
            Difference = AddValueOutput(nameof(Difference), GetDifference);
        }

        private async Task<int> GetDifference(IFlow flow)
        {
            return await flow.GetValueAsync<int>(A) - await flow.GetValueAsync<int>(B);
        }
    }
}