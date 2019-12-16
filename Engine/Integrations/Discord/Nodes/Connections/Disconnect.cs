using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Integrations.Data.Discord;

namespace Nodegem.Engine.Integrations.Discord.Nodes.Connections
{
    [DefinedNode("E1244B38-0218-454A-BF61-CFB1AF9F41B6", IsListenerOnly = true)]
    public class Disconnect : DiscordNode
    {
        
        public IFlowInputField In { get; set; }
        
        public Disconnect(IDiscordService discordService) : base(discordService)
        {
        }

        protected override void Define()
        {
            In = AddFlowInput(nameof(In), async flow =>
            {
                await Service.Client.StopAsync();
                return null;
            });
        }
    }
}