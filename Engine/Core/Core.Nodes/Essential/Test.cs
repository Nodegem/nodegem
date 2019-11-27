using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using ValueType = Nodegem.Common.Data.ValueType;

namespace Nodegem.Engine.Core.Nodes.Essential
{
    [DefinedNode("C2EBF203-23BF-4430-87CF-3230A8C185A6")]
    [NodeNamespace("Core.Essential")]
    public class Test : Node
    {
        [FieldAttributes(EnumOptions = typeof(ValueType))]
        public IValueInputField TestValue { get; set; }
        
        [FieldAttributes(ValueOptions = new object[] { 1, 2, 3 })]
        public IValueInputField TestValue2 { get; set; }
        
        protected override void Define()
        {
            TestValue = AddValueInput<string>(nameof(TestValue));
            TestValue2 = AddValueInput<int>(nameof(TestValue2));
        }
    }
}