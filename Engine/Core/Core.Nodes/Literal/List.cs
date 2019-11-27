using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nodegem.Common.Extensions;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Literal
{
    [DefinedNode("561056AC-1DE1-4AC7-9BB0-0E2F087E90A8")]
    [NodeNamespace("Core.Literal")]
    public class List : Node
    {
        [FieldAttributes("Values")]
        public IEnumerable<IValueInputField> Values { get; set; }
        
        [FieldAttributes("List")]
        public IValueOutputField NewList { get; set; }
        
        protected override void Define()
        {
            Values = InitializeValueInputList(nameof(Values), default(object));
            NewList = AddValueOutput("List", CreateListAsync);
        }

        private async Task<IList<object>> CreateListAsync(IFlow flow)
        {
            var values = await Values.SelectAsync(flow.GetValueAsync<object>);
            return values.ToList();
        }
    }
}