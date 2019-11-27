using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Nodegem.Engine.Integrations.Data.Discord;

namespace Nodegem.Engine.Integrations.Discord
{
    public class DiscordService : IDiscordService
    {
        
        public DiscordSocketClient Client { get; private set; }
        
        public async Task InitializeBotAsync(string botToken, DiscordSocketConfig config = null)
        {
            if (string.IsNullOrEmpty(botToken))
            {
                throw new ArgumentException("Bot token cannot be null.");
            }
            
            Client = new DiscordSocketClient(config ?? new DiscordSocketConfig
            {
                MessageCacheSize = 500
            });
            await Client.LoginAsync(TokenType.Bot, botToken);
        }
        
        public async Task StartBotAsync()
        {
            await Client.StartAsync();
        }

    }
}