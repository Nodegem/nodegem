using System;
using Bridge.Data;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.StreamNodes.Message
{
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