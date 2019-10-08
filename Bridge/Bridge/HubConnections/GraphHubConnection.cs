using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Data;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nodester.Data;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Data.Dto.MacroDtos;

namespace Nodester.Bridge.HubConnections
{
    public class GraphHubConnection : BaseHubConnection, IGraphHubConnection
    {
        public event OnRemoteExecuteGraph ExecuteGraphEvent;
        public event OnRemoteExecuteMacro ExecuteMacroEvent;

        private readonly ILogger<IGraphHubConnection> _logger;

        public GraphHubConnection(IOptions<AppConfig> config, ILogger<GraphHubConnection> logger) : base("/graphHub", config)
        {
            _logger = logger;
            Client.On<GraphDto>("RemoteExecuteGraphAsync", graph => { ExecuteGraphEvent?.Invoke(graph); });

            Client.On<MacroDto, string>("RemoteExecuteMacroAsync",
                (macro, inputId) => { ExecuteMacroEvent?.Invoke(macro, inputId); });
            
        }

        protected override Task OnReconnectingAsync(Exception ex)
        {
            _logger.LogError(ex, "Lost connection to server. Attempting reconnect...");
            return Task.CompletedTask;
        }

        protected override async Task OnReconnectedAsync(string newConnectionId)
        {
            await UpdateBridgeAsync(CancellationToken.None);
            _logger.LogInformation("Reestablished connection to server!");
        }

        public async Task UpdateBridgeAsync(CancellationToken cancelToken)
        {
            await Client.InvokeAsync("EstablishBridgeAsync", AppState.Instance.Info, cancelToken);
        }

        public override async Task StartAsync(CancellationToken cancelToken)
        {
            await base.StartAsync(cancelToken);
            await UpdateBridgeAsync(cancelToken);
        }

        public override async Task StopAsync(CancellationToken cancelToken)
        {
            await Client.InvokeAsync("RemoveBridgeAsync", cancelToken);
            await base.StopAsync(cancelToken);
        }

        public async Task OnGraphCompleteAsync(ExecutionErrorData? errorData = null)
        {
            await Client.InvokeAsync("OnGraphCompleteAsync", errorData);
        }
    }
}