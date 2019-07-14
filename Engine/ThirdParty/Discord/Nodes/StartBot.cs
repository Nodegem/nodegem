using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes
{
    [DefinedNode("Start Bot")]
    [NodeNamespace("Third Party.Discord")]
    public class StartBot : DiscordNode
    {
        
        public IFlowInputField In { get; set; }
        
        [FieldAttributes("Bot Token")]
        public IValueInputField BotToken { get; set; }

        public StartBot(IDiscordService service) : base(service)
        {
        }
        
        protected override void Define()
        {
            In = AddFlowInput(nameof(In), StartConnection);
            BotToken = AddValueInput<string>(nameof(BotToken));
        }

        private async Task<IFlowOutputField> StartConnection(IFlow flow)
        {
            await DiscordService.StartBotAsync(await flow.GetValueAsync<string>(BotToken));
            await Task.Delay(-1);
            return null;
        }
    }
}