using System;
using Bridge.Data;
using Discord;
using Discord.WebSocket;
using Mapster;
using Nodester.Engine.Data.Fields;
using Nodester.ThirdParty.Discord.Dtos;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes.Reaction_Events
{
    public class OnReactionAdded : ReactionEventListenerNode
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
                    await Graph.RunFlowAsync(On);
                }
                catch (Exception ex)
                {
                    
                }
            };
        }
    }
}