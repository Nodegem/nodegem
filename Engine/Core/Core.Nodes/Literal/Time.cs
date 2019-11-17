using Nodester.Common.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.Literal
{
    [DefinedNode]
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