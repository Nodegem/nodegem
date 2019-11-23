using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.Lists
{
    [DefinedNode]
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