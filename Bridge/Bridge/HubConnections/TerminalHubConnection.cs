using System.Threading.Tasks;
using Bridge.Data;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nodester.Common.Data;

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
            _logger.LogInformation(message, user);
            if (sendToClient)
            {
                await Client.InvokeAsync("Log", user, message);
            }
        }

        public async Task DebugLogAsync(User user, string message, bool isDebug, bool sendToClient)
        {
            if (isDebug)
            {
                _logger.LogDebug(message, user);
                if (sendToClient)
                {
                    await Client.InvokeAsync("DebugLog", user, message);
                }
            }
        }

        public async Task WarnLogAsync(User user, string message, bool sendToClient)
        {
            _logger.LogWarning(message, user);
            if (sendToClient)
            {
                await Client.InvokeAsync("WarnLog", user, message);
            }
        }

        public async Task ErrorLogAsync(User user, string message, bool sendToClient)
        {
            _logger.LogError(message, user);
            if (sendToClient)
            {
                await Client.InvokeAsync("ErrorLog", user, message);
            }
        }
    }
}