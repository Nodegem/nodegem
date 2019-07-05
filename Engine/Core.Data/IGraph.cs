using System;
using Nodester.Common.Data;
using Nodester.Engine.Data.Nodes;

namespace Nodester.Engine.Data
{
    public interface IGraph
    {
        string Name { get; }

        User User { get; }
        
        bool DebugMode { get; set; }
        
        INode GetNode(Guid nodeId);

        T GetConstant<T>(Guid key);

        void SetVariable(string key, object value);

        T GetVariable<T>(string key);
    }
}