using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nodester.Common.Data;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Exceptions;
using Nodester.Engine.Data.Links;
using Nodester.Engine.Data.Nodes;
using Nodester.Graph.Core.Extensions;
using Nodester.Graph.Core.Nodes.Essential;

namespace Nodester.Graph.Core
{
    public class FlowGraph : BaseGraph, IFlowGraph
    {
        private readonly IFlow _flow;
        private Dictionary<Guid, Constant> Constants { get; }

        public FlowGraph(
            Guid id,
            string name,
            Dictionary<Guid, INode> nodes, 
            Dictionary<Guid, Constant> constants, 
            User user)
            : base(id, name, nodes, user)
        {
            _flow = new Flow();
            Constants = constants;
            AssignNodesToGraph();
        }

        private void AssignNodesToGraph()
        {
            foreach (var node in Nodes)
            {
                node.Value.SetGraph(this);
            }
        }

        public override T GetConstant<T>(Guid key)
        {
            var result = base.GetConstant<T>(key);

            if (!Equals(result, default(T))) return result;
            
            if (Constants.TryGetValue(key, out var value))
            {
                return (T) value.Value;
            }

            return default;
        }

        public async Task RunAsync()
        {
            if (!Nodes.TryGetValueOfType(typeof(Start), out var start))
            {
                throw new StartNotFoundException(this);
            }

            await _flow.RunAsync(((Start) start.Value).StartFlow);
        }

        public IFlowLink GetConnection(string fromFieldKey)
        {
            return Nodes.Values.SelectMany(x => x.FlowConnections).FirstOrDefault(x => x.Source?.Key == fromFieldKey);
        }
    }
}