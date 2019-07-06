using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using Nodester.Graph.Core;
using Nodester.Graph.Core.Fields.Graph;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes
{
    [DefinedNode("Begin")]
    [NodeNamespace("Third Party.Discord")]
    public class Begin : Node
    {
        
        public FlowInput In { get; set; }
        public FlowOutput Start { get; set; }

        private readonly IDiscordService _discordService;

        public Begin(IDiscordService service)
        {
            _discordService = service;
        }
        
        protected override void Define()
        {
            In = AddFlowInput(nameof(In), StartConnection);
            Start = AddFlowOutput(nameof(Start));
        }

        private IFlowOutputField StartConnection(IFlow flow)
        {
        }
    }
}