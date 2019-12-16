using Nodegem.Engine.Core.Links.Graph;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Data.Links;

namespace Nodegem.Engine.Core.Fields.Graph
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