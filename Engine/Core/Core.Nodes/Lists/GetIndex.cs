using System.Collections.Generic;
using System.Threading.Tasks;
using Nodester.Common.Extensions;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.Lists
{
    [DefinedNode]
    [NodeNamespace("Core.Lists")]
    public class GetIndex : Node
    {
        public IValueInputField List { get; set; }
        public IValueInputField Value { get; set; }
        public IValueOutputField Index { get; set; }
        
        protected override void Define()
        {
            List = AddValueInput<IList<object>>(nameof(List));
            Value = AddValueInput<object>(nameof(Value));
            Index = AddValueOutput(nameof(Index), GetValueAsync);
        }

        private async Task<int> GetValueAsync(IFlow flow)
        {
            var list = await flow.GetValueAsync<IList<object>>(List);
            var value = await flow.GetValueAsync<object>(Value);
            return list.FindIndex(x => x == value);
        }
    }
}