using Nodester.Graph.Core;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes
{
    public abstract class DiscordNode : Node
    {
        protected IDiscordService DiscordService { get; }

        public DiscordNode(IDiscordService discordService)
        {
            DiscordService = discordService;
        }
    }
}