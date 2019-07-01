using Nodester.Graph.Core.Data.Fields;
using Nodester.Graph.Core.Data.Links;
using Nodester.Graph.Core.Links.Graph;

namespace Nodester.Graph.Core.Fields.Graph
{
    public class FlowOutput : FlowField, IFlowOutputField
    {
        public IFlowLink Connection { get; private set; }

        public FlowOutput(string key) : base(key)
        {
        }

        public void SetConnection(IFlowInputField input)
        {
            Connection = new FlowLink(this, input);
        }
    }
}