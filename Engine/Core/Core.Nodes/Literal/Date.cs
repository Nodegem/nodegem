using Nodester.Common.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.Literal
{
    [DefinedNode]
    [NodeNamespace("Core.Literal")]
    public class Date : Node
    {
        [FieldAttributes(ValueType.Date, AllowConnection = false)] public IValueInputField Value { get; set; }

        [FieldAttributes(ValueType.Date)] public IValueOutputField Output { get; set; }

        protected override void Define()
        {
            Value = AddValueInput(nameof(Value), default(DateTime));
            Output = AddValueOutput(nameof(Output), async flow => await flow.GetValueAsync<DateTime>(Value));
        }
    }
}