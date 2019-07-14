using Discord.WebSocket;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes
{
    [DefinedNode("On Message Received")]
    [NodeNamespace("Third Party.Discord")]
    public class OnMessageReceived : DiscordListenerNode
    {
        
        public IFlowOutputField On { get; set; }
        
        [FieldAttributes("Server Name")]
        public IValueOutputField ServerName { get; set; }
        public IValueOutputField Timestamp { get; set; }
        
        [FieldAttributes("Channel Name")]
        public IValueOutputField ChannelName { get; set; }
        public IValueOutputField Username { get; set; }
        public IValueOutputField Message { get; set; }
        
        public OnMessageReceived(IDiscordService discordService) : base(discordService)
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

        public override void SetupEventListeners()
        {
            DiscordService.Client.MessageReceived += async message =>
            {
                Message.SetValue(message.Content);
                Username.SetValue(message.Author.Username);
                ChannelName.SetValue(message.Channel.Name);
                Timestamp.SetValue(message.Timestamp.ToString());
                ServerName.SetValue(((SocketGuildChannel) message.Channel).Guild.Name);
                await Graph.RunFlowAsync(On);
            };
        }
    }
}