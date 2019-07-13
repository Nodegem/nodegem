using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.Graph
{
    [DefinedNode("Get Variable")]
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