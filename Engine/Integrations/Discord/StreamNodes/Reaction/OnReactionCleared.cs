using System;
using Nodegem.Common.Data.Interfaces;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Integrations.Data.Discord;

namespace Nodegem.Engine.Integrations.Discord.StreamNodes.Reaction
{
    [DefinedNode("76704501-428B-4121-AE6B-2E9C7E8557E7", IsListenerOnly = true)]
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
                    await SendErrorAsync(ex);
                }
            };
        }
    }
}