using System;
using Nodester.Graph.Core.Data;

namespace Nodester.Graph.Core.Data.Exceptions
{
    public class StartNotFoundException : GraphException
    {
        public StartNotFoundException(IGraph graph) : base("No 'Start' node found.", graph)
        {
        }
    }
}