using System;
using Bridge.Data;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes.Message_Events
{
    public class OnMessageReceived : MessageEventListenerNode
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
                    await Graph.RunFlowAsync(On);
                }
                catch (Exception ex)
                {
                    await SendErrorAsync(ex);
                }
            };
        }
    }
}