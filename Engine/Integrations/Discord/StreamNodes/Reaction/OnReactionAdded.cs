using System;
using Discord.WebSocket;
using Nodegem.Common.Data.Interfaces;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Integrations.Data.Discord;

namespace Nodegem.Engine.Integrations.Discord.StreamNodes.Reaction
{
    [DefinedNode("3D641ACF-9422-4825-A05E-6DFBF9FE3A03", IsListenerOnly = true)]
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
                    await SendErrorAsync(ex);
                }
            };
        }
    }
}