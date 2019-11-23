using System;
using Bridge.Data;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.StreamNodes.Message
{
    public class OnMessageReceived : MessageStreamNode
    {
        public OnMessageReceived(IDiscordService discordService, IGraphHubConnection graphHubConnection) : base(
            discordService, graphHubConnection)
        {
        }

        public override void SetupEventListeners()
        {
            DiscordService.Client.MessageReceived += async message =>
            {
                try
                {
                    SetBaseValues(message);
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