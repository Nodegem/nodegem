using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core
{
    public abstract class EventListenerNode : ListenerNode
    {
        public IFlowOutputField On { get; private set; }

        protected override void Define()
        {
            On = AddFlowOutput(nameof(On));
        }

        public abstract override void SetupEventListeners();
    }
}