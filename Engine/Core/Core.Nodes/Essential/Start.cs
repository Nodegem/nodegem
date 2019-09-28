using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.Essential
{
    [DefinedNode(IgnoreDisplay = true)]
    [NodeNamespace("Core.Essential")]
    public class Start : Node
    {

        public const string StartFullName = "Core.Essential.Start";
        
        public IFlowOutputField StartFlow { get; private set; }

        protected override void Define()
        {
            StartFlow = AddFlowOutput("Start");
        }
        
    }
}