using Discord;
using Discord.WebSocket;
using Nodester.Engine.Data.Fields;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes
{
    public abstract class DiscordEventListenerNode : DiscordListenerNode
    {
        public IFlowOutputField On { get; set; }
        public IValueOutputField ServerName { get; set; }
        public IValueOutputField Timestamp { get; set; }
        public IValueOutputField ChannelName { get; set; }
        public IValueOutputField Username { get; set; }
        public IValueOutputField Message { get; set; }

        protected DiscordEventListenerNode(IDiscordService discordService) : base(discordService)
        {
        }

        protected override void Define()
        {
            On = AddFlowOutput(nameof(On));
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