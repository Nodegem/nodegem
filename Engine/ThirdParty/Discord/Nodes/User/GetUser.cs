using System.Threading.Tasks;
using Discord.WebSocket;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Fields;
using Nodester.ThirdParty.Discord.Exceptions;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord.Nodes.User
{
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