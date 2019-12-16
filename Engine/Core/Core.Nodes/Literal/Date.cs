using Nodegem.Common.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Literal
{
    [DefinedNode("476FBF6C-FD25-4F95-891D-DCDBDDE6EDA8")]
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