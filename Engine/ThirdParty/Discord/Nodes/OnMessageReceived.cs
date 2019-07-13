using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Fields;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes
{
//    [DefinedNode("On Message Received")]
//    [NodeNamespace("Third Party.Discord")]
    public class OnMessageReceived : DiscordNode
    {
        
        public IFlowOutputField On { get; set; }
        
        public IValueOutputField Message { get; set; }
        
        public OnMessageReceived(IDiscordService discordService) : base(discordService)
        {
        }

        protected override void Define()
        {
            On = AddFlowOutput(nameof(On));
            Message = AddValueOutput(nameof(Message), HandleMessage);
        }

        private async Task<string> HandleMessage(IFlow flow)
        {
            return null;
        }
    }
}