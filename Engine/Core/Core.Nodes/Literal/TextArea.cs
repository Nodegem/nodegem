using Nodegem.Common.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Literal
{
    [DefinedNode("13129C93-450D-491E-8404-C23C2D4072EA")]
    [NodeNamespace("Core.Literal")]
    public class TextArea : Node
    {
        
        [FieldAttributes(ValueType.TextArea, AllowConnection = false)]
        public IValueInputField Value { get; set; }
        
        [FieldAttributes(ValueType.TextArea)]
        public IValueOutputField Output { get; set; }
        
        protected override void Define()
        {
            Value = AddValueInput(nameof(Value), default(string));
            Output = AddValueOutput<string>(nameof(Output),
                async flow => await flow.GetValueAsync<string>(Value));
        }
    }
}