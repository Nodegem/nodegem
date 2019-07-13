using System.Threading;
using System.Threading.Tasks;
using Bridge.Data;
using Microsoft.AspNetCore.SignalR.Client;
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

        public GraphHubConnection(IOptions<AppConfig> config) : base("/graphHub", config)
        {
            Client.On<GraphDto>("RemoteExecuteGraph", graph => { ExecuteGraphEvent?.Invoke(graph); });

            Client.On<MacroDto, string>("RemoteExecuteMacro",
                (macro, inputId) => { ExecuteMacroEvent?.Invoke(macro, inputId); });
        }

        public async Task StartAsync(BridgeInfo info, CancellationToken cancelToken)
        {
            await base.StartAsync(cancelToken);
            await Client.InvokeAsync("EstablishBridge", info, cancelToken);
        }
    }
}