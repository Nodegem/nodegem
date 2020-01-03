using System;
using Nodegem.Engine.Data.Nodes;

namespace Nodegem.Engine.Data.Exceptions
{
    public class FlowException : Exception
    {
        public IFlow Flow { get; }
        public INode Node { get; }
        public Exception OriginalException { get; }

        public FlowException(Exception ex, IFlow flow, INode node) : base(
            $"An exception occured while running graph: {node?.Graph?.Name}.", ex)
        {
            Flow = flow;
            Node = node;
            OriginalException = ex.InnerException;
        }
    }
}