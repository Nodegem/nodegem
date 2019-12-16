using System.Threading.Tasks;
using Nodegem.Engine.Core.Fields.Graph;
using Nodegem.Engine.Data.Attributes;

namespace Nodegem.Engine.Core.Nodes.Metadata.User
{
    [DefinedNode("A5E0088E-3A6F-49AC-98B7-819DA390B21E", Title = "Get Username")]
    [NodeNamespace("Core.Metadata.User")]
    public class GetUsername : Node
    {
        
        public ValueOutput Username { get; set; }
        
        protected override void Define()
        {
            Username = AddValueOutput(nameof(Username), flow => Task.FromResult(Graph.User.Username));
        }
    }
}