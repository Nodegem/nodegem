using Nodester.Graph.Core.Data.Fields;
using Nodester.Graph.Core.Data.Links;

namespace Nodester.Graph.Core.Links.Graph
{
    public class FlowLink : BaseLink<IFlowOutputField, IFlowInputField>, IFlowLink
    {
        public FlowLink(IFlowOutputField source, IFlowInputField dest) : base(source, dest)
        {
        }
    }
}