using System;
using Nodester.Common.Data;
using Nodester.Graph.Core.Data.Nodes;

namespace Nodester.Graph.Core.Data
{
    public interface IGraph
    {

        User User { get; }
        
        bool DebugMode { get; set; }
        
        INode GetNode(Guid nodeId);

        T GetConstant<T>(Guid key);

        void SetVariable(string key, object value);

        T GetVariable<T>(string key);
    }
}