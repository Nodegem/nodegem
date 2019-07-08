using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.Control
{
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