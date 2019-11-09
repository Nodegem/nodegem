using System.Threading.Tasks;
using Nodester.Engine.Data.Fields;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes.Connections
{
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