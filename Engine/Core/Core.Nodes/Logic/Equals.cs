using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.Logic
{
    [DefinedNode]
    [NodeNamespace("Core.Logic")]
    public class Equals : Node
    {
        public ValueInput A { get; private set; }
        public ValueInput B { get; private set; }

        [FieldAttributes("A = B")] public ValueOutput Result { get; private set; }

        protected override void Define()
        {
            A = AddValueInput<object>(nameof(A));
            B = AddValueInput<object>(nameof(B));
            Result = AddValueOutput<bool>(nameof(Result), Compare);
        }

        private async Task<bool> Compare(IFlow flow)
        {
            return (await flow.GetValueAsync<object>(A)).Equals(await flow.GetValueAsync<object>(B));
        }
    }
}