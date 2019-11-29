using System.Threading.Tasks;
using Discord.WebSocket;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Integrations.Data.Discord;
using Nodegem.Engine.Integrations.Data.Discord.Exceptions;

namespace Nodegem.Engine.Integrations.Discord.Nodes.User
{
    [DefinedNode("9CE37FD9-28E5-408A-8411-E3C17ACB7A3F")]
    public class GetUser : DiscordNode
    {
        
        public IValueInputField Username { get; set; }
        public IValueOutputField User { get; set; }
        
        public GetUser(IDiscordService discordService) : base(discordService)
        {
        }

        protected override void Define()
        {
            Username = AddValueInput<string>(nameof(Username));
            User = AddValueOutput(nameof(User), GetUserAsync);
        }

        private async Task<SocketUser> GetUserAsync(IFlow flow)
        {
            var username = await flow.GetValueAsync<string>(Username);
            var userSplit = username?.Split('#') ?? new string[0];
            if (userSplit.Length < 2)
            {
                throw new DiscordException("Username must contain discriminator", Graph);
            }

            return Service.Client.GetUser(userSplit[0], userSplit[1]);
        }
    }
}