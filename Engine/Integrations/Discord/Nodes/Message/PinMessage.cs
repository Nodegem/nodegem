using System;
using System.Threading.Tasks;
using Discord;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Integrations.Data.Discord;

namespace Nodegem.Engine.Integrations.Discord.Nodes.Message
{
    [DefinedNode("7660DDBE-AF99-4F77-9870-6FB4DFAD00B4")]
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
                Console.Error.WriteLine(ex.Message);
                return Failed;
            }
        }
    }
}