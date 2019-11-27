using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Data.Nodes;

namespace Nodegem.Engine.Data
{
    public interface IListenerGraph : IFlowGraph, IAsyncDisposable
    {
        IEnumerable<IListenerNode> ListenerNodes { get; }
        Task RunFlowAsync(IFlowOutputField outField);
    }
}