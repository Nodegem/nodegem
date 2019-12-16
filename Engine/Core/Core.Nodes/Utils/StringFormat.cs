using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nodegem.Common.Extensions;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using ValueType = Nodegem.Common.Data.ValueType;

namespace Nodegem.Engine.Core.Nodes.Utils
{
    [DefinedNode("EA345E62-371E-427D-9F49-F49ED16D0166", Title = "String Format")]
    [NodeNamespace("Core.Utils")]
    public class StringFormat : Node
    {
        public IValueInputField Format { get; set; }

        [FieldAttributes(nameof(Strings), ValueType.Text)]
        public IEnumerable<IValueInputField> Strings { get; set; }

        public IValueOutputField Formatted { get; set; }

        protected override void Define()
        {
            Format = AddValueInput(nameof(Format), "{0}");
            Strings = InitializeValueInputList<string>(nameof(Strings));
            Formatted = AddValueOutput(nameof(Formatted), FormatString);
        }

        private async Task<string> FormatString(IFlow flow)
        {
            var format = await flow.GetValueAsync<string>(Format);
            var strings = await Strings.SelectAsync(flow.GetValueAsync<string>);
            return string.Format(format, strings.ToArray());
        }
    }
}