using System.Threading.Tasks;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Integrations.Data.Discord;

namespace Nodegem.Engine.Integrations.Discord.Nodes.Connections
{
    [DefinedNode("F2A97BA5-3212-467B-AC92-57E3E792FC81", IsListenerOnly = true)]
    public class GetConnectionState : DiscordNode
    {
        public IValueOutputField ConnectionState { get; set; }

        public GetConnectionState(IDiscordService discordService) : base(discordService)
        {
        }

        protected override void Define()
        {
            ConnectionState = AddValueOutput(nameof(ConnectionState),
                flow => Task.FromResult(Service.Client.ConnectionState));
        }
    }
}