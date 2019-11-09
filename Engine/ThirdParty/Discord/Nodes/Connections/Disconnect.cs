using Nodester.Engine.Data.Fields;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes.Connections
{
    public class Disconnect : DiscordNode
    {
        
        public IFlowInputField In { get; set; }
        
        public Disconnect(IDiscordService discordService) : base(discordService)
        {
        }

        protected override void Define()
        {
            In = AddFlowInput(nameof(In), async flow =>
            {
                await Service.Client.StopAsync();
                return null;
            });
        }
    }
}