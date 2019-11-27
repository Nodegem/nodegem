using Nodegem.Common.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Literal
{
    [DefinedNode("43E5E76F-BEF5-4929-AFAD-78F3804D9691")]
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