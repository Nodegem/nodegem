using System;
using Nodegem.Engine.Data.Nodes;

namespace Nodegem.Engine.Data.Exceptions
{
    public class MacroFlowException : Exception
    {
        public IMacroFlow Flow { get; }
        public INode Node { get; }
        public Exception OriginalException { get; }

        public MacroFlowException(Exception ex, IMacroFlow flow, INode node) : base(
            $"An exception occured while running macro: {node?.Graph?.Name}", ex)
        {
            Flow = flow;
            Node = node;
            OriginalException = ex.InnerException;
        }
    }
}