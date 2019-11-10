using Bridge.Data;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes.Reaction_Events
{
    public class OnReactionAdded : DiscordEventListenerNode
    {
        public OnReactionAdded(IDiscordService discordService, IGraphHubConnection graphHubConnection) : base(
            discordService, graphHubConnection)
        {
        }

        public override void SetupEventListeners()
        {
            DiscordService.Client.ReactionAdded += async (cacheable, channel, reaction) =>
            {
                var cached = await cacheable.GetOrDownloadAsync();
                await Graph.RunFlowAsync(On);
            };
        }
    }
}