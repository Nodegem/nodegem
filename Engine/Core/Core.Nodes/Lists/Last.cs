using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Lists
{
    [DefinedNode("D9ED3635-B2FA-4DC2-8AE0-0B48E52A6C4F")]
    [NodeNamespace("Core.Lists")]
    public class Last : Node
    {
        public IValueInputField List { get; set; }
        
        [FieldAttributes("Last")]
        public IValueOutputField Value { get; set; }
        
        protected override void Define()
        {
            List = AddValueInput<IList<object>>(nameof(List));
            Value = AddValueOutput("Last", GetFirstAsync);
        }

        private async Task<object> GetFirstAsync(IFlow flow)
        {
            var list = await flow.GetValueAsync<IList<object>>(List);
            return list.Last();
        }
    }
}