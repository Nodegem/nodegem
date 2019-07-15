using Nodester.Engine.Data.Attributes;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes
{
    public class OnMessageReceived : DiscordEventListenerNode
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