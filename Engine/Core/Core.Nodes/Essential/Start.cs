using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.Essential
{
    [DefinedNode]
    [NodeNamespace("Core.Essential")]
    public class Start : Node
    {
        public IFlowOutputField StartFlow { get; private set; }

        protected override void Define()
        {
            StartFlow = AddFlowOutput("Start");
        }
        
    }
}