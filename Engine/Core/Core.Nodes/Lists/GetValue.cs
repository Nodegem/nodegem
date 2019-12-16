using System.Collections.Generic;
using System.Threading.Tasks;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Lists
{
    [DefinedNode("72852DAA-FCF7-4533-A36D-6E11095E87E6")]
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