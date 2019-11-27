using System.Collections.Generic;
using System.Threading.Tasks;
using Nodegem.Common.Extensions;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Lists
{
    [DefinedNode("28DC17D1-69C4-45BD-AC81-8D32C23BE8C3")]
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