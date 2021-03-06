using System;
using System.Collections.Generic;
using Nodegem.Common;
using Nodegem.Common.Data;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Nodes;

namespace Nodegem.Engine.Core
{

    public abstract class BaseGraph : IGraph
    {
        
        public Guid Id { get; }
        public BridgeInfo Bridge { get; }
        public string Name { get; }
        public User User { get; }
        public bool IsRunningLocally { get; set; } = true;
        
        protected Dictionary<string, object> Variables { get; }
        protected Dictionary<Guid, INode> Nodes { get; }

        protected BaseGraph(
            Guid id,
            BridgeInfo bridge,
            string name,
            Dictionary<Guid, INode> nodes,
            User user)
        {
            Id = id;
            Bridge = bridge;
            Name = name;
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