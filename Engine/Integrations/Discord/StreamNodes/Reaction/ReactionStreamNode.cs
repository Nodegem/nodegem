using Discord;
using Nodegem.Common.Data.Interfaces;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Integrations.Data.Discord;
using Nodegem.Engine.Integrations.Data.Discord.Dtos;

namespace Nodegem.Engine.Integrations.Discord.StreamNodes.Reaction
{
    public abstract class ReactionStreamNode : DiscordStreamNode
    {
        
        public IValueOutputField Message { get; set; }
        public IValueOutputField Channel { get; set; }
        
        public ReactionStreamNode(IDiscordService discordService, IGraphHubConnection graphHubConnection) : base(discordService, graphHubConnection)
        {
        }

        protected override void Define()
        {
            base.Define();
            Message = AddValueOutput<MessageDto>(nameof(Message));
            Channel = AddValueOutput<ChannelDto>(nameof(Channel));
        }

        public abstract override void SetupEventListeners();

        protected void SetBaseValues(IMessage message, IChannel channel)
        {
            Message.SetValue(message.Content);
            Channel.SetValue(channel);
        }
    }
}