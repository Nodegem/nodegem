using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Nodester.Bridge.HubConnections
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
                .Build();
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