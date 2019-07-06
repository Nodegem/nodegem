using System.Threading;
using System.Threading.Tasks;
using Bridge.Data;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nodester.Common.Data;

namespace Nodester.Bridge.HubConnections
{
    public class TerminalHubConnection : ITerminalHubConnection
    {
        
        private readonly HubConnection _connection;
        private readonly ILogger<TerminalHubConnection> _logger;
        
        public TerminalHubConnection(IOptions<AppConfig> config, ILogger<TerminalHubConnection> logger)
        {
            _logger = logger;
            
            var url = config.Value.Host;
            _connection = new HubConnectionBuilder()
                .WithUrl($"{url}/terminalHub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(AppState.Instance.Token.RawData);
                })
                .Build();
        }
        
        public async Task StartAsync(CancellationToken cancelToken)
        {
            await _connection.StartAsync(cancelToken);
        }

        public async Task StopAsync(CancellationToken cancelToken)
        {
            await _connection.StopAsync(cancelToken);
        }

        public void Dispose()
        {
            Task.FromResult(_connection.DisposeAsync());
        }

        public async Task LogAsync(User user, string message, bool sendToClient)
        {
            _logger.LogInformation(message, user);
            if (sendToClient)
            {
                await _connection.InvokeAsync("Log", user, message);
            }
        }

        public async Task DebugLogAsync(User user, string message, bool isDebug, bool sendToClient)
        {
            if (isDebug)
            {
                _logger.LogDebug(message, user);
                if (sendToClient)
                {
                    await _connection.InvokeAsync("DebugLog", user, message);
                }
            }
        }

        public async Task WarnLogAsync(User user, string message, bool sendToClient)
        {
            _logger.LogWarning(message, user);
            if (sendToClient)
            {
                await _connection.InvokeAsync("WarnLog", user, message);
            }
        }

        public async Task ErrorLogAsync(User user, string message, bool sendToClient)
        {
            _logger.LogError(message, user);
            if (sendToClient)
            {
                await _connection.InvokeAsync("ErrorLog", user, message);
            }
        }
    }
}