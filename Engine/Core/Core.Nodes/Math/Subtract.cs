using System.Threading.Tasks;
using Nodegem.Common.Data;
using Nodegem.Engine.Core.Fields.Graph;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;

namespace Nodegem.Engine.Core.Nodes.Math
{
    [DefinedNode("FC0537AF-A230-46A4-8A72-05F70823D27F")]
    [NodeNamespace("Core.Math")]
    public class Subtract : Node
    {
        [FieldAttributes(ValueType.Number)]
        public ValueInput A { get; private set; }
        
        [FieldAttributes(ValueType.Number)]
        public ValueInput B { get; private set; }

        [FieldAttributes("A - B", ValueType.Number)] 
        public ValueOutput Difference { get; private set; }

        protected override void Define()
        {
            A = AddValueInput<int>(nameof(A));
            B = AddValueInput<int>(nameof(B));
            Difference = AddValueOutput(nameof(Difference), GetDifference);
        }

        private async Task<int> GetDifference(IFlow flow)
        {
            return await flow.GetValueAsync<int>(A) - await flow.GetValueAsync<int>(B);
        }
    }
}