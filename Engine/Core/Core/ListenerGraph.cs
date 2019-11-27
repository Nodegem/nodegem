using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nodegem.Common;
using Nodegem.Common.Data;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Data.Nodes;

namespace Nodegem.Engine.Core
{
    public class ListenerGraph : FlowGraph, IListenerGraph
    {
        public IEnumerable<IListenerNode> ListenerNodes { get; }

        public ListenerGraph(Guid id, BridgeInfo bridge, string name, Dictionary<Guid, INode> nodes,
            Dictionary<Guid, Constant> constants,
            User user) : base(id, bridge, name, nodes, constants, user)
        {
            ListenerNodes = Nodes.Values.OfType<IListenerNode>();
        }

        public async Task RunFlowAsync(IFlowOutputField outField)
        {
            var flow = new Flow();
            await flow.RunAsync(outField);
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var listenerNode in ListenerNodes)
            {
                await listenerNode.DisposeAsync();
            }
        }
    }
}