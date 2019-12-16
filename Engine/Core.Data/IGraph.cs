using System;
using Nodegem.Common;
using Nodegem.Common.Data;
using Nodegem.Engine.Data.Nodes;

namespace Nodegem.Engine.Data
{
    public interface IGraph
    {
        Guid Id { get; }
        string Name { get; }

        User User { get; }
        BridgeInfo Bridge { get; }
        
        bool IsRunningLocally { get; set; }
        
        INode GetNode(Guid nodeId);

        T GetConstant<T>(Guid key);

        void SetVariable(string key, object value);

        T GetVariable<T>(string key);
    }
}