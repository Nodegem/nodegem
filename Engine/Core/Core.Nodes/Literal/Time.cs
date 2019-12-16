using Nodegem.Common.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Literal
{
    [DefinedNode("B0CD0597-8603-4905-A39F-0E425F35D78E")]
    [NodeNamespace("Core.Literal")]
    public class Time : Node
    {
        [FieldAttributes(ValueType.DateTime, AllowConnection = false)] public IValueInputField Value { get; set; }

        [FieldAttributes(ValueType.DateTime)] public IValueOutputField Output { get; set; }
        
        protected override void Define()
        {
            Value = AddValueInput(nameof(Value), default(DateTime));
            Output = AddValueOutput<DateTime>(nameof(Output),
                async flow => await flow.GetValueAsync<DateTime>(Value));
        }
    }
}