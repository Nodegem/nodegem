using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord
{
    public class DiscordService : IDiscordService
    {
        
        public DiscordSocketClient Client { get; private set; }
        
        public async Task InitializeBotAsync(string botToken, DiscordSocketConfig config = null)
        {
            Client = new DiscordSocketClient(config ?? new DiscordSocketConfig());
            await Client.LoginAsync(TokenType.Bot, botToken);
        }
        
        public async Task StartBotAsync()
        {
            await Client.StartAsync();
        }

    }
}