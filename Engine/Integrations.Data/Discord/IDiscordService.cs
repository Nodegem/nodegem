using System.Threading.Tasks;
using Discord.Rest;
using Discord.WebSocket;

namespace Nodegem.Engine.Integrations.Data.Discord
{
    public interface IDiscordService
    {
        DiscordRestClient RestClient { get; }
        DiscordSocketClient Client { get; }
        Task InitializeBotAsync(string token, DiscordSocketConfig config = null);
        Task StartBotAsync();
    }
}