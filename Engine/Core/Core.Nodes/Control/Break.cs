using System.Threading.Tasks;
using Nodegem.Engine.Core.Fields.Graph;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Control
{
    [DefinedNode("1B8691C5-5F2C-4343-AD37-A0DEDDFEC453")]
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