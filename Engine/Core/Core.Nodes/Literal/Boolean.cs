using Nodegem.Common.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Literal
{
    [DefinedNode("6D08A0A7-6595-4CC5-9B46-A4122292AB57")]
    [NodeNamespace("Core.Literal")]
    public class Boolean : Node
    {
        
        [FieldAttributes(ValueType.Boolean, AllowConnection = false)] public IValueInputField Value { get; set; }

        [FieldAttributes(ValueType.Boolean)] public IValueOutputField Output { get; set; }
        
        protected override void Define()
        {
            Value = AddValueInput(nameof(Value), default(bool));
            Output = AddValueOutput<bool>(nameof(Output),
                async flow => await flow.GetValueAsync<bool>(Value));
        }
    }
}