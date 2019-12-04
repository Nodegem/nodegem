using System;
using Discord.WebSocket;
using Nodegem.Common.Data.Interfaces;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Integrations.Data.Discord;

namespace Nodegem.Engine.Integrations.Discord.StreamNodes.Reaction
{
    [DefinedNode("74CB19C0-5A2F-4540-984E-889B73B68B9B", IsListenerOnly = true)]
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
                    await SendErrorAsync(ex);
                }
            };
        }
    }
}