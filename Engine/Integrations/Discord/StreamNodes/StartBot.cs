using System.Threading.Tasks;
using Nodegem.Common.Data.Interfaces;
using Nodegem.Common.Extensions;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Integrations.Data.Discord;

namespace Nodegem.Engine.Integrations.Discord.StreamNodes
{
    [DefinedNode("78AD22F8-9D51-4219-99B1-1C8C18E9CF13")]
    public class StartBot : DiscordStreamNode
    {
        public IFlowInputField In { get; set; }

        [FieldAttributes("Bot Token")] public IValueInputField BotToken { get; set; }

        public StartBot(IDiscordService service, IGraphHubConnection graphHubConnection) : base(service,
            graphHubConnection)
        {
        }

        protected override void Define()
        {
            base.Define();
            In = AddFlowInput(nameof(In), StartConnection);
            BotToken = AddValueInput<string>(nameof(BotToken));
        }

        public override void SetupEventListeners()
        {
            //Ignore
        }

        private async Task<IFlowOutputField> StartConnection(IFlow flow)
        {
            var botToken = await flow.GetValueAsync<string>(BotToken);
            await DiscordService.InitializeBotAsync(botToken);
            Graph.ListenerNodes.ForEach(x => x.SetupEventListeners());
            await DiscordService.StartBotAsync();
            return Out;
        }

        public override async ValueTask DisposeAsync()
        {
            if (DiscordService.Client != null)
            {
                await DiscordService.Client.LogoutAsync();
                DiscordService.Client.Dispose();
            }
        }
    }
}