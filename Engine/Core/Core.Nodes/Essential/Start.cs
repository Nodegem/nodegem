using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Essential
{
    [DefinedNode("5E135A6F-9B6A-4CE1-BF84-584275AB4410", IgnoreDisplay = true)]
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