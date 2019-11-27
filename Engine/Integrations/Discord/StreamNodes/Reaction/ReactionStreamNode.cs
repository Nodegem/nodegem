using Discord;
using Nodegem.Common.Data.Interfaces;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Integrations.Data.Discord;
using Nodegem.Engine.Integrations.Discord.Dtos;

namespace Nodegem.Engine.Integrations.Discord.StreamNodes.Reaction
{
    [DefinedNode("F6328E1D-0B87-423F-B074-F383BB7DBA62")]
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