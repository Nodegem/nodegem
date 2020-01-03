using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Data.Nodes;

namespace Nodegem.Engine.Core.Fields
{
    public class FlowField : BaseField, IFlowField
    {
        protected FlowField(string key) : base(key)
        {
        }
        
        protected FlowField(string key, INode node) : base(key, node)
        {
        }
    }
}