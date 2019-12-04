using Nodegem.Common.Data.Interfaces;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Integrations.Data.Discord;

namespace Nodegem.Engine.Integrations.Discord.StreamNodes.Connections
{
    [DefinedNode("2AFD944B-FCEC-4F17-B96F-D4498F8A87FA", IsListenerOnly = true)]
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