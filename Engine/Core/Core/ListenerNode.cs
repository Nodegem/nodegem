using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Nodes;

namespace Nodester.Graph.Core
{
    public abstract class ListenerNode : Node, IListenerNode
    {
        public new IListenerGraph Graph => base.Graph as IListenerGraph;
        
        protected abstract override void Define();

        public abstract void SetupEventListeners();

        public virtual ValueTask DisposeAsync()
        {
            return new ValueTask();
        }
    }
}