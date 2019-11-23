using System.Collections.Generic;
using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.Lists
{
    [DefinedNode]
    [NodeNamespace("Core.Lists")]
    public class GetValue : Node
    {
        public IValueInputField List { get; set; }
        public IValueInputField Index { get; set; }
        public IValueOutputField Value { get; set; }
        
        protected override void Define()
        {
            List = AddValueInput<IList<object>>(nameof(List));
            Index = AddValueInput(nameof(Index), default(int));
            Value = AddValueOutput(nameof(Value), GetValueAsync);
        }

        private async Task<object> GetValueAsync(IFlow flow)
        {
            var list = await flow.GetValueAsync<IList<object>>(List);
            var index = await flow.GetValueAsync<int>(Index);
            return list[index];
        }
    }
}