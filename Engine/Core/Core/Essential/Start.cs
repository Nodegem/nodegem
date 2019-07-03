using Nodester.Engine.Data.Attributes;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Essential
{
    [DefinedNode]
    [NodeNamespace("Core.Essential")]
    public class Start : Node
    {
        public FlowOutput StartFlow { get; private set; }

        protected override void Define()
        {
            StartFlow = AddFlowOutput("Start");
        }
        
    }
}