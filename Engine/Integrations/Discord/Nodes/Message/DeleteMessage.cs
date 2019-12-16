using System;
using System.Threading.Tasks;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Integrations.Data.Discord;

namespace Nodegem.Engine.Integrations.Discord.Nodes.Message
{
    [DefinedNode("47D82939-AF32-4290-8D09-369912431E7E")]
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
                Console.Error.WriteLine(ex.Message);
                return Failed;
            }
        }
    }
}