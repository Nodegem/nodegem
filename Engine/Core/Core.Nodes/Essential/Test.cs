using System.Runtime.Serialization;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Essential
{

    public enum TestEnum
    {
        [FriendlyName("Value 1")]
        Value,
        [FriendlyName("Value 2")]
        Value1,
        [FriendlyName("Value 3")]
        Value2
    }
    
    [DefinedNode("C2EBF203-23BF-4430-87CF-3230A8C185A6")]
    [NodeNamespace("Core.Essential")]
    public class Test : Node
    {
        [FieldAttributes(EnumOptions = typeof(TestEnum))]
        public IValueInputField TestValue2 { get; set; }
        
        protected override void Define()
        {
            TestValue2 = AddValueInput(nameof(TestValue2), 1);
        }
    }
}