using Nodester.Engine.Data.Attributes;
using Nodester.Graph.Core;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes
{
    [DefinedNode]
    [NodeNamespace("Third Party.Discord")]
    public abstract class DiscordListenerNode : ListenerNode
    {
        
        protected IDiscordService DiscordService { get; }

        protected DiscordListenerNode(IDiscordService discordService)
        {
            DiscordService = discordService;
        }

        protected abstract override void Define();

    }
}