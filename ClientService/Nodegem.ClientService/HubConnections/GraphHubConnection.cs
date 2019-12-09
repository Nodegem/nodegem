using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nodegem.Common.Data;
using Nodegem.Common.Data.Interfaces;
using Nodegem.Common.Dto;

namespace Nodegem.ClientService.HubConnections
{
    public class GraphHubConnection : BaseHubConnection, IGraphHubConnection
    {
        public event OnDisposeListeners DisposeListenersEvent;
        public event OnRemoteExecuteGraph ExecuteGraphEvent;
        public event OnRemoteExecuteMacro ExecuteMacroEvent;
        public event OnUserUpdated UserUpdatedEvent;

        private readonly ILogger<IGraphHubConnection> _logger;

        public GraphHubConnection(IOptions<AppConfig> config, ILogger<GraphHubConnection> logger) : base("/graphHub",
            config)
        {
            _logger = logger;

            Client.On("DisposeListenersAsync", () => DisposeListenersEvent?.Invoke());
            Client.On<GraphDto>("RemoteExecuteGraphAsync", graph => { ExecuteGraphEvent?.Invoke(graph); });
            Client.On<MacroDto, string>("RemoteExecuteMacroAsync",
                (macro, inputId) => { ExecuteMacroEvent?.Invoke(macro, inputId); });
            Client.On<TokenDto>("UpdatedUserAsync", token => UserUpdatedEvent?.Invoke(token));
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

        public async Task SendGraphErrorAsync(ExecutionErrorData errorData, CancellationToken cancelToken)
        {
            await Client.InvokeAsync("OnGraphErrorAsync", errorData, cancelToken);
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