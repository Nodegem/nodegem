using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Fields;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes.Message
{
    public abstract class BaseMessageNode : DiscordNode
    {
        public IFlowInputField In { get; set; }
        public IFlowOutputField Success { get; set; }
        public IFlowOutputField Failed { get; set; }
        public IValueInputField GuildId { get; set; }
        public IValueInputField ChannelId { get; set; }
        public IValueInputField MessageId { get; set; }
        
        protected BaseMessageNode(IDiscordService discordService) : base(discordService)
        {
        }
        
        protected override void Define()
        {
            In = AddFlowInput(nameof(In), ExecuteActionAsync);
            Success = AddFlowOutput(nameof(Success));
            Failed = AddFlowOutput(nameof(Failed));
            GuildId = AddValueInput<ulong>(nameof(GuildId));
            ChannelId = AddValueInput<ulong>(nameof(ChannelId));
            MessageId = AddValueInput<ulong>(nameof(MessageId));
        }

        protected abstract Task<IFlowOutputField> ExecuteActionAsync(IFlow flow);
    }
}