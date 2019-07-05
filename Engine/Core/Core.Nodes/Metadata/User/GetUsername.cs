using Nodester.Engine.Data.Attributes;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.Metadata.User
{
    [DefinedNode("Get Username")]
    [NodeNamespace("Core.Metadata.User")]
    public class GetUsername : Node
    {
        
        public ValueOutput Username { get; set; }
        
        protected override void Define()
        {
            Username = AddValueOutput(nameof(Username), flow => Graph.User.Username);
        }
    }
}