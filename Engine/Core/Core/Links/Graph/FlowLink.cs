using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Data.Links;

namespace Nodegem.Engine.Core.Links.Graph
{
    public class FlowLink : BaseLink<IFlowOutputField, IFlowInputField>, IFlowLink
    {
        public FlowLink(IFlowOutputField source, IFlowInputField dest) : base(source, dest)
        {
        }
    }
}