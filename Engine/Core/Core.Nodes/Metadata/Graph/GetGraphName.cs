using System.Threading.Tasks;
using Nodester.Engine.Data.Attributes;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.Metadata.Graph
{
    [DefinedNode("Get Graph Name")]
    [NodeNamespace("Core.Metadata.Graph")]
    public class GetGraphName : Node
    {
        
        public ValueOutput Name { get; set; }
        
        protected override void Define()
        {
            Name = AddValueOutput(nameof(Name), flow => Task.FromResult(Graph.Name));
        }
    }
}