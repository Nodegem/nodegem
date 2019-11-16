using Nodester.Common.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.Literals
{
    [DefinedNode]
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