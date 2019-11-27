using System.Threading.Tasks;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Control
{
    [NodeNamespace("Core.Control")]
    public abstract class LoopNode : Node
    {
        public IFlowInputField In { get; private set; }
        public IFlowOutputField Out { get; private set; }
        public IFlowOutputField Block { get; private set; }

        protected override void Define()
        {
            In = AddFlowInput(nameof(In), OnLoop);
            Out = AddFlowOutput(nameof(Out));
            Block = AddFlowOutput(nameof(Block));
        }

        protected abstract Task<IFlowOutputField> OnLoop(IFlow flow);
    }
}