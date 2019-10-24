using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nodester.Common.Data;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Fields;
using Nodester.Engine.Data.Nodes;

namespace Nodester.Graph.Core
{
    public class ListenerGraph : FlowGraph, IListenerGraph
    {
        public IEnumerable<IListenerNode> ListenerNodes { get; }

        public ListenerGraph(Guid id, string name, Dictionary<Guid, INode> nodes, Dictionary<Guid, Constant> constants,
            User user) : base(id, name, nodes, constants, user)
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