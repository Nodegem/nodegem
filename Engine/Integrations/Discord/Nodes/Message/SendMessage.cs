using System.Threading.Tasks;
using Discord;
using Nodegem.Common.Data;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Integrations.Data.Discord;

namespace Nodegem.Engine.Integrations.Discord.Nodes.Message
{
    [DefinedNode("603E657E-7AA5-4C1F-A7EE-9B1B48F40F0D")]
    public class SendMessage : DiscordNode
    {
        public IFlowInputField In { get; set; }
        public IFlowOutputField Out { get; set; }
        
        [FieldAttributes(ValueType.Text)]
        public IValueInputField BotToken { get; set; }
        
        [FieldAttributes(ValueType.Text)]
        public IValueInputField ChannelId { get; set; }
        
        [FieldAttributes(ValueType.TextArea)]
        public IValueInputField Message { get; set; }
        
        public SendMessage(IDiscordService discordService) : base(discordService)
        {
        }

        protected override void Define()
        {
            In = AddFlowInput(nameof(In), AddMessageAsync);
            Out = AddFlowOutput(nameof(Out));
            BotToken = AddValueInput<string>(nameof(BotToken));
            ChannelId = AddValueInput<ulong>(nameof(ChannelId));
            Message = AddValueInput<string>(nameof(Message));
        }

        private async Task<IFlowOutputField> AddMessageAsync(IFlow flow)
        {
            var channelId = await flow.GetValueAsync<ulong>(ChannelId);
            var message = await flow.GetValueAsync<string>(Message);
            var botToken = await flow.GetValueAsync<string>(BotToken);
            
            var restClient = Service.RestClient;
            await restClient.LoginAsync(TokenType.Bot, botToken);
            var channel = await restClient.GetChannelAsync(channelId);
            if (channel is ITextChannel textChannel)
            {
                await textChannel.SendMessageAsync(message);
            }
            return Out;
        }
    }
}