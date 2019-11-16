using System;
using System.Threading.Tasks;
using Discord;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Fields;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes.Message_Events
{
    public class PinMessage : BaseMessageNode
    {
        
        public PinMessage(IDiscordService discordService) : base(discordService)
        {
        }

        protected override async Task<IFlowOutputField> ExecuteActionAsync(IFlow flow)
        {
            try
            {
                var guildId = await flow.GetValueAsync<ulong>(GuildId);
                var channelId = await flow.GetValueAsync<ulong>(ChannelId);
                var messageId = await flow.GetValueAsync<ulong>(MessageId);
                var guild = Service.Client.GetGuild(guildId);
                var channel = guild.GetTextChannel(channelId);
                var message = await channel.GetMessageAsync(messageId) as IUserMessage;
                await message.PinAsync();
                return Success;
            }
            catch (Exception ex)
            {
                return Failed;
            }
        }
    }
}