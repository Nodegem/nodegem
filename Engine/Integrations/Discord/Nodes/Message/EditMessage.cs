using System;
using System.Threading.Tasks;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Integrations.Data.Discord;

namespace Nodegem.Engine.Integrations.Discord.Nodes.Message
{
    [DefinedNode("604B0A1D-15B2-4C43-AF35-6785C45A7AB5")]
    public class EditMessage : BaseMessageNode
    {
        public EditMessage(IDiscordService discordService) : base(discordService)
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