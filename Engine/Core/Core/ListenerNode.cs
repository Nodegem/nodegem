using Nodester.Engine.Data;
using Nodester.Engine.Data.Nodes;

namespace Nodester.Graph.Core
{
    public abstract class ListenerNode : Node, IListenerNode
    {
        public new IListenerGraph Graph => base.Graph as IListenerGraph;
        

        protected override void Define()
        {
        }

        public abstract void SetupEventListeners();
    }
}