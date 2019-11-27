using System.Threading.Tasks;
using Mapster;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Integrations.Data.Discord;
using Nodegem.Engine.Integrations.Discord.Dtos;

namespace Nodegem.Engine.Integrations.Discord.Nodes.Message
{
    [DefinedNode("40119A73-D067-43B2-A7C3-BE8E0DA822DE")]
    public class GetMessage : DiscordNode
    {
        public IValueInputField GuildId { get; set; }
        public IValueInputField ChannelId { get; set; }
        public IValueInputField MessageId { get; set; }
        public IValueOutputField Message { get; set; }

        public GetMessage(IDiscordService discordService) : base(discordService)
        {
        }

        protected override void Define()
        {
            GuildId = AddValueInput<ulong>(nameof(GuildId));
            ChannelId = AddValueInput<ulong>(nameof(ChannelId));
            MessageId = AddValueInput<ulong>(nameof(MessageId));
            Message = AddValueOutput<MessageDto>(nameof(Message), GetMessageAsync);
        }

        private async Task<MessageDto> GetMessageAsync(IFlow flow)
        {
            var guildId = await flow.GetValueAsync<ulong>(GuildId);
            var channelId = await flow.GetValueAsync<ulong>(ChannelId);
            var messageId = await flow.GetValueAsync<ulong>(MessageId);
            var guild = Service.Client.GetGuild(guildId);
            return (await guild.GetTextChannel(channelId).GetMessageAsync(messageId)).Adapt<MessageDto>();
        }
    }
}