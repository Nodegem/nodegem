using Nodegem.Common.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Literal
{
    [DefinedNode("0E005CE7-7EF8-4E15-B036-34BD954C6550")]
    [NodeNamespace("Core.Literal")]
    public class Text : Node
    {
        
        [FieldAttributes(ValueType.Text, AllowConnection = false)]
        public IValueInputField Value { get; set; }
        
        [FieldAttributes(ValueType.Text)]
        public IValueOutputField Output { get; set; }
        
        protected override void Define()
        {
            Value = AddValueInput(nameof(Value), default(string));
            Output = AddValueOutput<string>(nameof(Output),
                async flow => await flow.GetValueAsync<string>(Value));
        }
    }
}