using System;
using Nodester.Graph.Core.Data;

namespace Nodester.Graph.Core.Data.Exceptions
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