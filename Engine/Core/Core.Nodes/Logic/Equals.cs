using System.Threading.Tasks;
using Nodegem.Engine.Core.Fields.Graph;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;

namespace Nodegem.Engine.Core.Nodes.Logic
{
    [DefinedNode("48B781C0-7697-4D14-903E-9B1227CC5CC0")]
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