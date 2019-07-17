using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes.Message_Events
{
    public class OnMessageReceived : MessageEventListenerNode
    {
        
        public OnMessageReceived(IDiscordService discordService) : base(discordService)
        {
        }

        public override void SetupEventListeners()
        {
            DiscordService.Client.MessageReceived += async message =>
            {
                SetBaseValues(message);
                await Graph.RunFlowAsync(On);
            };
        }
    }
}