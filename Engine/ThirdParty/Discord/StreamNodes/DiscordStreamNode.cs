using Bridge.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Graph.Core;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.StreamNodes
{
    [NodeNamespace("Third Party.Discord")]
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