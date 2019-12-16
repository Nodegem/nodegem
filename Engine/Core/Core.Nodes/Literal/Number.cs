using Nodegem.Common.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Literal
{
    [DefinedNode("FA667D2E-3C40-40EF-B261-F4843FBE6987")]
    [NodeNamespace("Core.Literal")]
    public class Number : Node
    {
        
        [FieldAttributes(ValueType.Number, AllowConnection = false)]
        public IValueInputField Value { get; set; }
        
        [FieldAttributes(ValueType.Number)]
        public IValueOutputField Output { get; set; }
        
        protected override void Define()
        {
            Value = AddValueInput(nameof(Value), default(double));
            Output = AddValueOutput<double>(nameof(Output),
                async flow => await flow.GetValueAsync<double>(Value));
        }
    }
}