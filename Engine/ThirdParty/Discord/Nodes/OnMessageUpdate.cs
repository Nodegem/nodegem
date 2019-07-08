using Nodester.Engine.Data.Attributes;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes
{
    [DefinedNode("On Message Update")]
    public class OnMessageUpdate : DiscordNode
    {
        
        public OnMessageUpdate(IDiscordService discordService) : base(discordService)
        {
        }

        protected override void Define()
        {
            
        }
    }
}