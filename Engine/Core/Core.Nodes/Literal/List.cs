using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nodester.Common.Extensions;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.Literal
{
    [DefinedNode]
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