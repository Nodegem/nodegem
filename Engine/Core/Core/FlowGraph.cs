using System;
using System.Collections.Generic;
using System.Linq;
using Nodester.Common.Data;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Exceptions;
using Nodester.Engine.Data.Links;
using Nodester.Engine.Data.Nodes;
using Nodester.Graph.Core.Extensions;
using Nodester.Graph.Core.Essential;

namespace Nodester.Graph.Core
{
    public class FlowGraph : BaseGraph, IFlowGraph
    {
        private readonly IFlow _flow;
        private Dictionary<Guid, Constant> Constants { get; }

        public FlowGraph(Dictionary<Guid, INode> nodes, Dictionary<Guid, Constant> constants, User user)
            : base(nodes, user)
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

        public void Run()
        {
            if (!Nodes.TryGetValueOfType(typeof(Start), out var start))
            {
                throw new StartNotFoundException(this);
            }

            _flow.Run(((Start) start.Value).StartFlow);
        }

        public IFlowLink GetConnection(string fromFieldKey)
        {
            return Nodes.Values.SelectMany(x => x.FlowConnections).FirstOrDefault(x => x.Source?.Key == fromFieldKey);
        }
    }
}