using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nodegem.ClientService.Data;
using Nodegem.Common.Data;

namespace Nodegem.ClientService.HubConnections
{
    public class TerminalHubConnection : BaseHubConnection, ITerminalHubConnection
    {
        private readonly ILogger<TerminalHubConnection> _logger;

        public TerminalHubConnection(IOptions<AppConfig> config, ILogger<TerminalHubConnection> logger) : base(
            "/terminalHub", config)
        {
            _logger = logger;
        }

        public async Task LogAsync(User user, string graphId, string message, bool sendToClient)
        {
            _logger.LogInformation($"Logging: {message} User ID: {user.Id} Graph ID: {graphId}");
            if (sendToClient)
            {
                await Client.InvokeAsync("LogAsync", user, graphId, message);
            }
        }

        public async Task WarnLogAsync(User user, string graphId, string message, bool sendToClient)
        {
            _logger.LogWarning($"Logging: {message} User ID: {user.Id} Graph ID: {graphId}");
            if (sendToClient)
            {
                await Client.InvokeAsync("WarnLogAsync", user, graphId, message);
            }
        }

        public async Task ErrorLogAsync(User user, string graphId, string message, bool sendToClient)
        {
            _logger.LogError($"Logging: {message} User ID: {user.Id} Graph ID: {graphId}");
            if (sendToClient)
            {
                await Client.InvokeAsync("ErrorLogAsync", user, graphId, message);
            }
        }
    }
}