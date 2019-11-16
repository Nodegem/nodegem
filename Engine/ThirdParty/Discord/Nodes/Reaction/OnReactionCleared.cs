using System;
using Bridge.Data;
using Discord.WebSocket;
using Nodester.Engine.Data.Fields;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes.Reaction_Events
{
    public class OnReactionCleared : ReactionEventListenerNode
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
                    await Graph.RunFlowAsync(On);
                }
                catch (Exception ex)
                {
                    
                }
            };
        }
    }
}