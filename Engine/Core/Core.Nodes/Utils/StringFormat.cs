using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nodester.Common.Extensions;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using ValueType = Nodester.Common.Data.ValueType;

namespace Nodester.Graph.Core.Nodes.Utils
{
    [DefinedNode("String Format")]
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