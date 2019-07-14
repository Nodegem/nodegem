using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nodester.Engine.Data.Fields;
using Nodester.Engine.Data.Nodes;

namespace Nodester.Engine.Data
{
    public interface IListenerGraph : IFlowGraph, IAsyncDisposable
    {
        IEnumerable<IListenerNode> ListenerNodes { get; }
        Task RunFlowAsync(IFlowOutputField outField);
    }
}