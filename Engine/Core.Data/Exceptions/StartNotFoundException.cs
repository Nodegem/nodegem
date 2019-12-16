namespace Nodegem.Engine.Data.Exceptions
{
    public class StartNotFoundException : GraphException
    {
        public StartNotFoundException(IGraph graph) : base("No 'Start' node found.", graph)
        {
        }
    }
}