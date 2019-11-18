using System;
using Bridge.Data;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.StreamNodes.Reaction
{
    public class OnReactionCleared : ReactionStreamNode
    {
        
        public OnReactionCleared(IDiscordService discordService, IGraphHubConnection graphHubConnection) : base(discordService, graphHubConnection)
        {
        }

        public override void SetupEventListeners()
        {
            DiscordService.Client.ReactionsCleared += async (cache, channel) =>
            {
                var message = await cache.GetOrDownloadAsync();

                try
                {
                    await Graph.RunFlowAsync(Out);
                }
                catch (Exception ex)
                {
                    
                }
            };
        }
    }
}