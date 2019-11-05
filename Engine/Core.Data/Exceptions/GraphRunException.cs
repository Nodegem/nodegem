namespace Nodester.Engine.Data.Exceptions
{
    public class GraphRunException : GraphException
    {
        public GraphRunException(string message, IGraph graph) : base(message, graph)
        {
        }
    }
}