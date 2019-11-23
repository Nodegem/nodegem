using Nodester.Common.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.Literal
{
    [DefinedNode]
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