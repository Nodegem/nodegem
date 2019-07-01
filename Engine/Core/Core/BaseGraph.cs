using System;
using System.Collections.Generic;
using Nodester.Common.Data;
using Nodester.Graph.Core.Data;
using Nodester.Graph.Core.Data.Nodes;

namespace Nodester.Graph.Core
{
    public abstract class BaseGraph : IGraph
    {
        public User User { get; }
        public bool DebugMode { get; set; } 
        
        protected Dictionary<string, object> Variables { get; }
        protected Dictionary<Guid, INode> Nodes { get; }

        protected BaseGraph(
            Dictionary<Guid, INode> nodes,
            User user)
        {
            Nodes = nodes;
            Variables = new Dictionary<string, object>();
            User = user;
        }
        
        public virtual T GetConstant<T>(Guid key)
        {
            if (User.Constants.TryGetValue(key, out var value))
            {
                return (T) value.Value;
            }

            return default;
        }

        public virtual void SetVariable(string key, object value)
        {
            Variables[key] = value;
        }

        public virtual T GetVariable<T>(string key)
        {
            if (Variables.TryGetValue(key, out var value))
            {
                return (T) value;
            }

            return default;
        }

        public virtual INode GetNode(Guid nodeId)
        {
            return Nodes[nodeId];
        }
    }
}