using System;
using Bridge.Data;
using Discord.WebSocket;
using Nodester.Engine.Data.Fields;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.StreamNodes.Reaction
{
    public class OnReactionRemoved : ReactionStreamNode
    {
        
        public IValueOutputField Reaction { get; set; }
        
        public OnReactionRemoved(IDiscordService discordService, IGraphHubConnection graphHubConnection) : base(discordService, graphHubConnection)
        {
        }

        protected override void Define()
        {
            base.Define();
            Reaction = AddValueOutput<SocketReaction>(nameof(Reaction));
        }

        public override void SetupEventListeners()
        {
            DiscordService.Client.ReactionRemoved += async (cache, channel, reaction) =>
            {
                var message = await cache.GetOrDownloadAsync();
                Reaction.SetValue(reaction);

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