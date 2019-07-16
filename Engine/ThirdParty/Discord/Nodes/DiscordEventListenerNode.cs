using Nodester.Engine.Data.Attributes;
using Nodester.Graph.Core;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes
{
    [DefinedNode]
    [NodeNamespace("Third Party.Discord")]
    public abstract class DiscordEventListenerNode : EventListenerNode
    {

        protected IDiscordService DiscordService { get; }

        protected DiscordEventListenerNode(IDiscordService discordService)
        {
            DiscordService = discordService;
        }

    }
}