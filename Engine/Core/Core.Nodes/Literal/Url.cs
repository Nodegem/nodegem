using Nodester.Common.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.Literal
{
    [DefinedNode]
    [NodeNamespace("Core.Literal")]
    public class Url : Node
    {
        
        [FieldAttributes(ValueType.Url, AllowConnection = false)]
        public IValueInputField Value { get; set; }
        
        [FieldAttributes(ValueType.Url)]
        public IValueOutputField Output { get; set; }
        
        protected override void Define()
        {
            Value = AddValueInput(nameof(Value), default(string));
            Output = AddValueOutput<string>(nameof(Output),
                async flow => await flow.GetValueAsync<string>(Value));
        }
    }
}