using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Nodegem.ClientService.HubConnections
{
    public abstract class BaseHubConnection : IDisposable
    {
        
        protected HubConnection Client { get; }
        
        protected BaseHubConnection(string urlPath, IOptions<AppConfig> config)
        {
            var host = config.Value.Host;
            Client = new HubConnectionBuilder()
                .WithUrl(new Uri($"{host}{urlPath}"),
                    options =>
                    {
                        options.AccessTokenProvider = () => Task.FromResult(AppState.Instance.Token.RawData);
                    })
                .AddNewtonsoftJsonProtocol()
                .WithAutomaticReconnect()
                .Build();

            Client.Reconnecting += OnReconnectingAsync;
            Client.Reconnected += OnReconnectedAsync;
        }

        protected virtual Task OnReconnectingAsync(Exception ex)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnReconnectedAsync(string newConnectionId)
        {
            return Task.CompletedTask;
        }
        
        public virtual async Task StartAsync(CancellationToken cancelToken)
        {
            await Client.StartAsync(cancelToken);
        }

        public virtual async Task StopAsync(CancellationToken cancelToken)
        {
            await Client.StopAsync(cancelToken);
        }
        
        public virtual void Dispose()
        {
            Task.FromResult(Client.DisposeAsync());
        }
    }
}