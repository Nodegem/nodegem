using Nodester.Graph.Core;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes
{
    public abstract class DiscordListenerNode : ListenerNode
    {
        protected IDiscordService DiscordService { get; }

        protected DiscordListenerNode(IDiscordService discordService)
        {
            DiscordService = discordService;
        }
    }
}