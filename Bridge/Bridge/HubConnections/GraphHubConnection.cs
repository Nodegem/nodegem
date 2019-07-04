using System.Threading;
using System.Threading.Tasks;
using Bridge.Data;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using Nodester.Common.Data;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Data.Dto.MacroDtos;

namespace Nodester.Bridge.HubConnections
{
    public class GraphHubConnection : IGraphHubConnection
    {
        
        public event OnRemoteExecuteGraph ExecuteGraphEvent;
        public event OnRemoteExecuteMacro ExecuteMacroEvent;

        private readonly HubConnection _connection;
        
        public GraphHubConnection(IOptions<AppConfig> config)
        {
            var url = config.Value.Host;
            _connection = new HubConnectionBuilder()
                .WithUrl($"{url}/graphHub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(AppState.Instance.Token.RawData);
                })
                .Build();

            _connection.On<GraphDto>("RemoteExecuteGraph", (graph) =>
            {
                ExecuteGraphEvent?.Invoke(graph);
            });
            
            _connection.On<MacroDto, string>("RemoteExecuteMacro", (macro, inputId) =>
            {
                ExecuteMacroEvent?.Invoke(macro, inputId);
            });
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