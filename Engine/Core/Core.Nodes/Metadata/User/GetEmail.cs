using System.Threading.Tasks;
using Nodegem.Engine.Core.Fields.Graph;
using Nodegem.Engine.Data.Attributes;

namespace Nodegem.Engine.Core.Nodes.Metadata.User
{
    [DefinedNode("348ECD85-5593-448E-B167-3C74F2C9FA3A", Title = "Get Email")]
    [NodeNamespace("Core.Metadata.User")]
    public class GetEmail : Node
    {
        
        public ValueOutput Email { get; set; }
        
        protected override void Define()
        {
            Email = AddValueOutput(nameof(Email), flow => Task.FromResult(Graph.User.Email));
        }
    }
}