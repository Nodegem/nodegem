using Nodester.Engine.Data.Attributes;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.Metadata.User
{
    [DefinedNode("Get Email")]
    [NodeNamespace("Core.Metadata.User")]
    public class GetEmail : Node
    {
        
        public ValueOutput Email { get; set; }
        
        protected override void Define()
        {
            Email = AddValueOutput(nameof(Email), flow => Graph.User.Email);
        }
    }
}