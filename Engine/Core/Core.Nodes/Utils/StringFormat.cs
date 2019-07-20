using System.Collections.Generic;
using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.Utils
{
    [DefinedNode("String Format")]
    [NodeNamespace("Core.Utils")]
    public class StringFormat : Node
    {
        
        public IValueInputField Format { get; set; }
        
        [FieldAttributes(Indefinite = true)]
        public IValueInputField Strings { get; set; }
        
        public IValueOutputField Formatted { get; set; }
        
        protected override void Define()
        {
            Format = AddValueInput(nameof(Format), "{0}");
            Strings = AddValueInput(nameof(Strings), new[] { "" });
            Formatted = AddValueOutput(nameof(Formatted), FormatString);
        }

        private async Task<string> FormatString(IFlow flow)
        {
            var format = await flow.GetValueAsync<string>(Format);
            var strings = await flow.GetValueAsync<IEnumerable<string>>(Strings);
            return string.Format(format, strings);
        }
        
    }
}