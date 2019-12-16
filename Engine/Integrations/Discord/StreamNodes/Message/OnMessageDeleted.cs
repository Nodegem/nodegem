using System;
using Nodegem.Common.Data.Interfaces;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Integrations.Data.Discord;

namespace Nodegem.Engine.Integrations.Discord.StreamNodes.Message
{
    [DefinedNode("28355967-FD38-47E8-A0B6-6F19FB494CCB", IsListenerOnly = true)]
    public class OnMessageDeleted : MessageStreamNode
    {
        public OnMessageDeleted(IDiscordService discordService, IGraphHubConnection graphHubConnection) : base(
            discordService, graphHubConnection)
        {
        }

        public override void SetupEventListeners()
        {
            DiscordService.Client.MessageDeleted += async (before, message) =>
            {
                try
                {
                    SetBaseValues(await before.GetOrDownloadAsync());
                    await Graph.RunFlowAsync(Out);
                }
                catch (Exception ex)
                {
                    await SendErrorAsync(ex);
                }
            };
        }
    }
}