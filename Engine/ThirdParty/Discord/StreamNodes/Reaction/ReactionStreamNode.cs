using Bridge.Data;
using Discord;
using Nodester.Engine.Data.Fields;
using Nodester.ThirdParty.Discord.Dtos;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.StreamNodes.Reaction
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