using System;
using Bridge.Data;
using Nodester.Engine.Data.Fields;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes.Message_Events
{
    public class OnMessageUpdated : MessageEventListenerNode
    {
        public IValueOutputField OldMessage { get; set; }

        public OnMessageUpdated(IDiscordService discordService, IGraphHubConnection graphHubConnection) : base(
            discordService, graphHubConnection)
        {
        }

        protected override void Define()
        {
            OldMessage = AddValueOutput<string>(nameof(OldMessage));
            base.Define();
        }

        public override void SetupEventListeners()
        {
            DiscordService.Client.MessageUpdated += async (before, after, channel) =>
            {
                try
                {
                    var oldMessage = await before.GetOrDownloadAsync();
                    OldMessage.SetValue(oldMessage.Content);
                    SetBaseValues(after);
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