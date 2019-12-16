using System.Threading.Tasks;
using Nodegem.Engine.Core.Fields.Graph;
using Nodegem.Engine.Data.Attributes;

namespace Nodegem.Engine.Core.Nodes.Metadata.Graph
{
    [DefinedNode("C62D5A76-1D18-4D5A-87F8-DD4ED3FEEC70", Title = "Get Graph Name")]
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