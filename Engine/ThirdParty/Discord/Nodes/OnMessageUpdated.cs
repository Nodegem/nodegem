using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes
{
    public class OnMessageUpdated : DiscordEventListenerNode
    {
        
        public IValueOutputField OldMessage { get; set; }
        
        public OnMessageUpdated(IDiscordService discordService) : base(discordService)
        {
        }

        protected override void Define()
        {
            OldMessage = AddValueOutput<string>(nameof(OldMessage));
            base.Define();
        }

        public override void SetupEventListeners()
        {
            DiscordService.Client.MessageUpdated += async (before, after, channel) =>
            {
                var oldMessage = await before.GetOrDownloadAsync();
                OldMessage.SetValue(oldMessage.Content);
                SetBaseValues(after);
                await Graph.RunFlowAsync(On);
            };
        }
    }
}