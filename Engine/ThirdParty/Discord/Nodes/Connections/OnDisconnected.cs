using Nodester.Engine.Data.Fields;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes.Connections
{
    public class OnDisconnected : DiscordEventListenerNode
    {
        
        public IValueOutputField Exception { get; set; }
        
        public OnDisconnected(IDiscordService discordService) : base(discordService)
        {
        }

        protected override void Define()
        {
            Exception = AddValueOutput<string>(nameof(Exception));
            base.Define();
        }

        public override void SetupEventListeners()
        {
            DiscordService.Client.Disconnected += async ex =>
            {
                Exception.SetValue(ex.Message);
                await Graph.RunFlowAsync(On);
            };
        }
    }
}