using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes.Connections
{
    public class OnConnected : DiscordEventListenerNode
    {
        public OnConnected(IDiscordService discordService) : base(discordService)
        {
        }

        public override void SetupEventListeners()
        {
            DiscordService.Client.Connected += async () => { await Graph.RunFlowAsync(On); };
        }
    }
}