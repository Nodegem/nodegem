using System.Threading.Tasks;
using Discord.WebSocket;

namespace ThirdParty.Data.Discord
{
    public interface IDiscordService
    {
        DiscordSocketClient Client { get; }
        Task InitializeBotAsync(string token, DiscordSocketConfig config = null);
        Task StartBotAsync();
    }
}