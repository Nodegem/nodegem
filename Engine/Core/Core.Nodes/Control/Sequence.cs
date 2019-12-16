using System.Collections.Generic;
using System.Threading.Tasks;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Control
{
    [DefinedNode("82D1C69A-C6A1-4D5D-AEF6-1D8B66730750")]
    [NodeNamespace("Core.Control")]
    public class Sequence : Node
    {
        
        public IFlowInputField In { get; set; }
        
        [FieldAttributes(nameof(Out))]
        public IEnumerable<IFlowOutputField> Out { get; set; }
        
        protected override void Define()
        {
            In = AddFlowInput(nameof(In), RunSequenceAsync);
            Out = InitializeFlowOutputList(nameof(Out));
        }

        private async Task<IFlowOutputField> RunSequenceAsync(IFlow flow)
        {
            foreach (var flowOut in Out)
            {
                await flow.RunAsync(flowOut);
            }

            return null;
        }
    }
}