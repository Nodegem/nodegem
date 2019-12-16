using Discord;
using Discord.WebSocket;
using Nodegem.Common.Data.Interfaces;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Integrations.Data.Discord;

namespace Nodegem.Engine.Integrations.Discord.StreamNodes.Message
{
    public abstract class MessageStreamNode : DiscordStreamNode
    {
        public IValueOutputField ServerName { get; set; }
        public IValueOutputField Timestamp { get; set; }
        public IValueOutputField ChannelName { get; set; }
        public IValueOutputField Username { get; set; }
        public IValueOutputField Message { get; set; }

        protected MessageStreamNode(IDiscordService discordService, IGraphHubConnection graphHubConnection) :
            base(discordService, graphHubConnection)
        {
        }

        protected override void Define()
        {
            base.Define();
            Message = AddValueOutput<string>(nameof(Message));
            Username = AddValueOutput<string>(nameof(Username));
            ChannelName = AddValueOutput<string>(nameof(ChannelName));
            Timestamp = AddValueOutput<string>(nameof(Timestamp));
            ServerName = AddValueOutput<string>(nameof(ServerName));
        }

        protected void SetBaseValues(IMessage message)
        {
            Message.SetValue(message.Content);
            Username.SetValue(message.Author.Username);
            ChannelName.SetValue(message.Channel.Name);
            Timestamp.SetValue(message.Timestamp.ToString());
            ServerName.SetValue(((SocketGuildChannel) message.Channel).Guild.Name);
        }
    }
}