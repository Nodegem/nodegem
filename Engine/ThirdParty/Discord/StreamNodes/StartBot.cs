using System.Threading.Tasks;
using Bridge.Data;
using Nodester.Common.Extensions;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.StreamNodes
{
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