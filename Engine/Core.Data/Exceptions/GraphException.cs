using System;

namespace Nodegem.Engine.Data.Exceptions
{
    public class GraphException : Exception
    {
        public IGraph Graph { get; }

        public GraphException(string message, IGraph graph) : base(message)
        {
            Graph = graph;
        }
    }
}