using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes.Message_Events
{
    public class OnMessageDeleted : MessageEventListenerNode
    {
        
        public OnMessageDeleted(IDiscordService discordService) : base(discordService)
        {
        }

        public override void SetupEventListeners()
        {
            DiscordService.Client.MessageDeleted += async (before, message) =>
            {
                SetBaseValues(await before.GetOrDownloadAsync());
                await Graph.RunFlowAsync(On);
            };
        }
    }
}