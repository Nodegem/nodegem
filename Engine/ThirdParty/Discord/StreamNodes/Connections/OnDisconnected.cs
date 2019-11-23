using Bridge.Data;
using Nodester.Engine.Data.Fields;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.StreamNodes.Connections
{
    public class OnDisconnected : DiscordStreamNode
    {
        public IValueOutputField Exception { get; set; }

        public OnDisconnected(IDiscordService discordService, IGraphHubConnection graphHubConnection) : base(
            discordService, graphHubConnection)
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
                await Graph.RunFlowAsync(Out);
            };
        }
    }
}