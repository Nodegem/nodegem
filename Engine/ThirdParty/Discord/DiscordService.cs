using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord
{
    public class DiscordService : IDiscordService
    {
        
        private DiscordSocketClient Client { get; set; }
        

        public async Task StartBotAsync(string botToken, DiscordSocketConfig config = null)
        {
            Client = new DiscordSocketClient(config ?? new DiscordSocketConfig());
            await Client.LoginAsync(TokenType.Bot, botToken);
            await Client.StartAsync();
        }
        
    }
}