using Nodester.Engine.Data.Attributes;
using Nodester.Graph.Core;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes
{
    [ListenerNode]
    public abstract class DiscordNode : Node
    {
        protected IDiscordService DiscordService { get; }

        protected DiscordNode(IDiscordService discordService)
        {
            DiscordService = discordService;
        }
    }
}