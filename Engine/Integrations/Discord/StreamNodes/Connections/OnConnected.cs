using Nodegem.Common.Data.Interfaces;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Integrations.Data.Discord;

namespace Nodegem.Engine.Integrations.Discord.StreamNodes.Connections
{
    [DefinedNode("32CDBDD7-658F-47CB-A0BC-C9DEDEB41D38")]
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