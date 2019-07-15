using System.Threading.Tasks;
using Nodester.Common.Data.Interfaces;
using Nodester.Common.Extensions;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes
{
    [DefinedNode("Start Bot (Discord)")]
    [NodeNamespace("Third Party.Discord")]
    public class StartBot : DiscordListenerNode
    {
        
        public IFlowInputField In { get; set; }
        
        [FieldAttributes("Bot Token")]
        public IValueInputField BotToken { get; set; }

        private readonly ITerminalHubService _terminalHub;

        public StartBot(IDiscordService service, ITerminalHubService terminalHub) : base(service)
        {
            _terminalHub = terminalHub;
        }
        
        protected override void Define()
        {
            In = AddFlowInput(nameof(In), StartConnection);
            BotToken = AddValueInput<string>(nameof(BotToken));
        }

        public override void SetupEventListeners()
        {
            //Ignore
        }

        private async Task<IFlowOutputField> StartConnection(IFlow flow)
        {
            await LogMessage("Initializing bot...");
            await DiscordService.InitializeBotAsync(await flow.GetValueAsync<string>(BotToken));
            await LogMessage("Configuring event listeners...");
            Graph.ListenerNodes.ForEach(x => x.SetupEventListeners());
            await DiscordService.StartBotAsync();
            await LogMessage("Bot is running!");
            return null;
        }

        private async Task LogMessage(string message)
        {
            await _terminalHub.LogAsync(Graph.User, message, !Graph.IsRunningLocally);
        }

        public override async ValueTask DisposeAsync()
        {
            await LogMessage("Disposing bot...");
            await DiscordService.Client.LogoutAsync();
            DiscordService.Client.Dispose();
        }
    }
}