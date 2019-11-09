using Nodester.Engine.Data.Attributes;
using Nodester.Graph.Core;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes
{
    [DefinedNode]
    [NodeNamespace("Third Party.Discord")]
    public abstract class DiscordNode : Node
    {
        
        protected IDiscordService Service { get; }
        
        protected DiscordNode(IDiscordService discordService)
        {
            Service = discordService;
        }
    }
}