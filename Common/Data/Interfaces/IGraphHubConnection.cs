using System;
using System.Threading;
using System.Threading.Tasks;
using Nodester.Common.Dto;
using Nodester.Data;

namespace Bridge.Data
{

    public delegate Task OnRemoteExecuteGraph(GraphDto graph);
    public delegate Task OnRemoteExecuteMacro(MacroDto graph, string startingFlowFieldId);
    
    public interface IGraphHubConnection : IDisposable
    {

        event OnRemoteExecuteGraph ExecuteGraphEvent;
        event OnRemoteExecuteMacro ExecuteMacroEvent;
        
        Task StartAsync(CancellationToken cancelToken);

        Task StopAsync(CancellationToken cancelToken);

        Task UpdateBridgeAsync(CancellationToken cancelToken);
        Task SendGraphErrorAsync(ExecutionErrorData errorData, CancellationToken cancelToken);

        Task OnGraphCompleteAsync(ExecutionErrorData? errorData = null);

    }
}