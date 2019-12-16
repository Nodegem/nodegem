using Nodegem.Engine.Core;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Integrations.Data.Discord;

namespace Nodegem.Engine.Integrations.Discord.Nodes
{
    [NodeNamespace("Integrations.Discord")]
    public abstract class DiscordNode : Node
    {
        
        protected IDiscordService Service { get; }
        
        protected DiscordNode(IDiscordService discordService)
        {
            Service = discordService;
        }
    }
}