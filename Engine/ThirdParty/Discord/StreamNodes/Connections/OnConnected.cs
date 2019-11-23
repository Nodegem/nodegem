using Bridge.Data;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.StreamNodes.Connections
{
    public class OnConnected : DiscordStreamNode
    {
        public OnConnected(IDiscordService discordService, IGraphHubConnection graphHubConnection) : base(
            discordService, graphHubConnection)
        {
        }

        public override void SetupEventListeners()
        {
            DiscordService.Client.Connected += async () => { await Graph.RunFlowAsync(Out); };
        }
    }
}