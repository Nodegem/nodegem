using Nodegem.Common.Data.Interfaces;
using Nodegem.Engine.Core;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Integrations.Data.Discord;

namespace Nodegem.Engine.Integrations.Discord.StreamNodes
{
    [NodeNamespace("Integrations.Discord")]
    public abstract class DiscordStreamNode : StreamNode
    {
        protected IDiscordService DiscordService { get; }

        protected DiscordStreamNode(IDiscordService discordService, IGraphHubConnection graphHubConnection) :
            base(graphHubConnection)
        {
            DiscordService = discordService;
        }
    }
}