using System;
using Nodegem.Common.Data.Interfaces;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Integrations.Data.Discord;

namespace Nodegem.Engine.Integrations.Discord.StreamNodes.Message
{
    [DefinedNode("EF3E7E04-7604-4492-815B-E42B91843D2D")]
    public class OnMessageUpdated : MessageStreamNode
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