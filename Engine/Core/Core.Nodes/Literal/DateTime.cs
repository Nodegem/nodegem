using Nodegem.Common.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Literal
{
    [DefinedNode("C8EEC48E-86EB-48A4-A405-E53D0DBC04CD")]
    [NodeNamespace("Core.Literal")]
    public class DateTime : Node
    {
        
        [FieldAttributes(ValueType.DateTime, AllowConnection = false)] public IValueInputField Value { get; set; }

        [FieldAttributes(ValueType.DateTime)] public IValueOutputField Output { get; set; }
        
        protected override void Define()
        {
            Value = AddValueInput(nameof(Value), System.DateTime.UtcNow);
            Output = AddValueOutput(nameof(Output),
                async flow => await flow.GetValueAsync<System.DateTime>(Value));
        }
    }
}