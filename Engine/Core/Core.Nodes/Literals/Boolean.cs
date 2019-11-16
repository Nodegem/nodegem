using Nodester.Common.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.Literals
{
    [DefinedNode]
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