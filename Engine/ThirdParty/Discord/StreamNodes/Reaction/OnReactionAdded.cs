using System;
using Bridge.Data;
using Discord.WebSocket;
using Nodester.Engine.Data.Fields;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.StreamNodes.Reaction
{
    public class OnReactionAdded : ReactionStreamNode
    {
        
        public IValueOutputField Reaction { get; set; }
        
        public OnReactionAdded(IDiscordService discordService, IGraphHubConnection graphHubConnection) : base(
            discordService, graphHubConnection)
        {
        }

        protected override void Define()
        {
            base.Define();
            Reaction = AddValueOutput<SocketReaction>(nameof(Reaction));
        }

        public override void SetupEventListeners()
        {
            DiscordService.Client.ReactionAdded += async (cache, channel, reaction) =>
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