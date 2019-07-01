using System.Threading;
using System.Threading.Tasks;
using Bridge.Data;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace Nodester.Bridge.HubConnections
{
    public class GraphHubConnection : IGraphHubConnection
    {

        private readonly HubConnection _connection;
        
        public GraphHubConnection(IOptions<AppConfig> config)
        {
            var url = config.Value.Host;
            _connection = new HubConnectionBuilder()
                .WithUrl($"{url}/graphHub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(AppState.Instance.Token.AccessToken);
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
    }
}