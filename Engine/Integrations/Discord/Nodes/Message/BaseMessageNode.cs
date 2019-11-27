using System.Threading.Tasks;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Integrations.Data.Discord;

namespace Nodegem.Engine.Integrations.Discord.Nodes.Message
{
    [DefinedNode("B6844B57-6173-4183-8C5C-F95E6E6BFB70")]
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