using System.Threading.Tasks;
using Bridge.Data;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nodester.Common.Data;
using Nodester.Common.Extensions;

namespace Nodester.Bridge.HubConnections
{
    public class TerminalHubConnection : BaseHubConnection, ITerminalHubConnection
    {
        private readonly ILogger<TerminalHubConnection> _logger;

        public TerminalHubConnection(IOptions<AppConfig> config, ILogger<TerminalHubConnection> logger) : base(
            "/terminalHub", config)
        {
            _logger = logger;
        }

        public async Task LogAsync(User user, string message, bool sendToClient)
        {
            _logger.LogInformation($"Logging: {message} User ID: {user.Id}");
            if (sendToClient)
            {
                var truncatedMessage = message.TruncateAtWord(1024);
                await Client.InvokeAsync("LogAsync", user, truncatedMessage);
            }
        }

        public async Task DebugLogAsync(User user, string message, bool isDebug, bool sendToClient)
        {
            if (isDebug)
            {
                _logger.LogDebug($"Logging: {message} User ID: {user.Id}");
                if (sendToClient)
                {
                    var truncatedMessage = message.TruncateAtWord(1024);
                    await Client.InvokeAsync("DebugLogAsync", user, message);
                }
            }
        }

        public async Task WarnLogAsync(User user, string message, bool sendToClient)
        {
            _logger.LogWarning($"Logging: {message} User ID: {user.Id}");
            if (sendToClient)
            {
                var truncatedMessage = message.TruncateAtWord(1024);
                await Client.InvokeAsync("WarnLogAsync", user, truncatedMessage);
            }
        }

        public async Task ErrorLogAsync(User user, string message, bool sendToClient)
        {
            _logger.LogError($"Logging: {message} User ID: {user.Id}");
            if (sendToClient)
            {
                var truncatedMessage = message.TruncateAtWord(1024);
                await Client.InvokeAsync("ErrorLogAsync", user, truncatedMessage);
            }
        }
    }
}