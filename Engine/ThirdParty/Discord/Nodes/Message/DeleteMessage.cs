using System;
using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Fields;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes.Message
{
    public class DeleteMessage : BaseMessageNode
    {
        public DeleteMessage(IDiscordService discordService) : base(discordService)
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
                await channel.DeleteMessageAsync(messageId);
                return Success;
            }
            catch (Exception ex)
            {
                return Failed;
            }
        }
    }
}