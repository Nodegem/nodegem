using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Lists
{
    [DefinedNode("CA3C2568-8239-42D9-8B17-F8EB628C5ABE")]
    [NodeNamespace("Core.Lists")]
    public class First : Node
    {
        public IValueInputField List { get; set; }
        
        [FieldAttributes("First")]
        public IValueOutputField Value { get; set; }
        
        protected override void Define()
        {
            List = AddValueInput<IList<object>>(nameof(List));
            Value = AddValueOutput("First", GetFirstAsync);
        }

        private async Task<object> GetFirstAsync(IFlow flow)
        {
            var list = await flow.GetValueAsync<IList<object>>(List);
            return list.First();
        }
    }
}