using System.Threading.Tasks;
using Nodegem.Engine.Core.Fields.Graph;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;

namespace Nodegem.Engine.Core.Nodes.Graph
{
    [DefinedNode("95DF6F9E-198E-4D99-9A16-118D25D38D16", Title = "Get Variable")]
    [NodeNamespace("Core.Graph")]
    public class GetVariable : Node
    {
        public ValueInput Variable { get; private set; }
        public ValueOutput Value { get; private set; }

        protected override void Define()
        {
            Variable = AddValueInput<string>(nameof(Variable));
            Value = AddValueOutput<object>(nameof(Value), GetValue);
        }

        private async Task<object> GetValue(IFlow flow)
        {
            return Graph.GetVariable<object>(await flow.GetValueAsync<string>(Variable));
        }
    }
}