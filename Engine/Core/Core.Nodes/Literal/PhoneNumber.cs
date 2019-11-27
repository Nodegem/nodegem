using Nodegem.Common.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Literal
{
    [DefinedNode("5C50E233-AD94-4CA5-8EE0-C8011C665258")]
    [NodeNamespace("Core.Literal")]
    public class PhoneNumber : Node
    {
        
        [FieldAttributes(ValueType.PhoneNumber, AllowConnection = false)]
        public IValueInputField Value { get; set; }
        
        [FieldAttributes(ValueType.PhoneNumber)]
        public IValueOutputField Output { get; set; }
        
        protected override void Define()
        {
            Value = AddValueInput(nameof(Value), default(string));
            Output = AddValueOutput<string>(nameof(Output),
                async flow => await flow.GetValueAsync<string>(Value));
        }
    }
}