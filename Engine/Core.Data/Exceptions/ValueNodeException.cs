using System;
using Nodegem.Engine.Data.Nodes;

namespace Nodegem.Engine.Data.Exceptions
{
    public class ValueNodeException : Exception
    {
        
        public INode Node { get; }
        
        public ValueNodeException(Exception ex, INode node) : base(
            $"An exception occured while retrieving value from node: {node?.Title}", ex)
        {
            Node = node;
        }
    }
}