using System.Threading.Tasks;
using Mapster;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Fields;
using Nodester.ThirdParty.Discord.Dtos;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes.Message_Events
{
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