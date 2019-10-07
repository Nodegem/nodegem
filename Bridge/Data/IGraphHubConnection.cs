using System;
using System.Threading;
using System.Threading.Tasks;
using Nodester.Data;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Data.Dto.MacroDtos;

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

        Task OnGraphCompleteAsync(ExecutionErrorData? errorData = null);

    }
}