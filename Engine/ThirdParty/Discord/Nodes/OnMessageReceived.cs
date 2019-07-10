using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes
{
//    [DefinedNode("On Message Received")]
//    [NodeNamespace("Third Party.Discord")]
    public class OnMessageReceived : DiscordNode
    {
        
        private IValueOutputField Message { get; set; }
        
        public OnMessageReceived(IDiscordService discordService) : base(discordService)
        {
        }

        protected override void Define()
        {
            Message = AddValueOutput(nameof(Message), HandleMessage);
        }

        private string HandleMessage(IFlow flow)
        {
//            DiscordService.Client.MessageReceived += async message =>
//            {
//                await Task.CompletedTask;
//            };

            return null;
        }
    }
}