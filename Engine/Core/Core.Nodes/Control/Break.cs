using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
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

        private Task<IFlowOutputField> BreakLoop(IFlow flow)
        {
            flow.BreakLoop();
            return null;
        }
    }
}