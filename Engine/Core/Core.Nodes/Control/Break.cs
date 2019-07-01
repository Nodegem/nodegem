using Nodester.Graph.Core.Data;
using Nodester.Graph.Core.Data.Attributes;
using Nodester.Graph.Core.Data.Fields;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.Control
{
    [DefinedNode]
    [NodeNamespace("Core.Control")]
    public class Break : Node
    {
        
        public FlowInput In { get; set; }
        
        protected override void Define()
        {
            In = AddFlowInput(nameof(In), BreakLoop);
        }

        private IFlowOutputField BreakLoop(IFlow flow)
        {
            flow.BreakLoop();
            return null;
        }
    }
}