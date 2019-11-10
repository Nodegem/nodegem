using System.Collections.Generic;
using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.Control
{
    [DefinedNode]
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