namespace Nodegem.Engine.Data.Exceptions
{
    public class GraphBuildException : GraphException
    {
        public GraphBuildException(string message, IGraph graph) : base(message, graph)
        {
        }
    }
}