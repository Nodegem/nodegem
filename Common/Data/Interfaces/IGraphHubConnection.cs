using System;
using System.Threading;
using System.Threading.Tasks;
using Nodegem.Common.Dto;

namespace Nodegem.Common.Data.Interfaces
{

    public delegate Task OnRemoteExecuteGraph(GraphDto graph);
    public delegate Task OnRemoteExecuteMacro(MacroDto graph, string startingFlowFieldId);
    public delegate Task OnDisposeListeners();
    public delegate void OnUserUpdated(TokenDto token);
    
    public interface IGraphHubConnection : IDisposable
    {

        event OnDisposeListeners DisposeListenersEvent;
        event OnRemoteExecuteGraph ExecuteGraphEvent;
        event OnRemoteExecuteMacro ExecuteMacroEvent;
        event OnUserUpdated UserUpdatedEvent;
        
        Task StartAsync(CancellationToken cancelToken);

        Task StopAsync(CancellationToken cancelToken);

        Task UpdateBridgeAsync(CancellationToken cancelToken);
        Task SendGraphErrorAsync(ExecutionErrorData errorData, CancellationToken cancelToken);

        Task OnGraphCompleteAsync(ExecutionErrorData? errorData = null);

    }
}