using System;
using Nodester.Common.Data;
using Nodester.Data;
using Nodester.Engine.Data.Nodes;

namespace Nodester.Engine.Data
{
    public interface IGraph
    {
        Guid Id { get; }
        string Name { get; }

        User User { get; }
        BridgeInfo Bridge { get; }
        
        bool DebugMode { get; set; }
        bool IsRunningLocally { get; set; }
        
        INode GetNode(Guid nodeId);

        T GetConstant<T>(Guid key);

        void SetVariable(string key, object value);

        T GetVariable<T>(string key);
    }
}