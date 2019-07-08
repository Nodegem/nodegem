using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes
{
    [DefinedNode("Begin")]
    [NodeNamespace("Third Party.Discord")]
    public class Begin : DiscordNode
    {
        
        public IFlowInputField In { get; set; }
        public IFlowOutputField Start { get; set; }

        public Begin(IDiscordService service) : base(service)
        {
        }
        
        protected override void Define()
        {
            In = AddFlowInput(nameof(In), StartConnection);
            Start = AddFlowOutput(nameof(Start));
        }

        private Task<IFlowOutputField> StartConnection(IFlow flow)
        {
            return null;
        }
    }
}