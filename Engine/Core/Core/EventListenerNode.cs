using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Data;
using Nodester.Data;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core
{
    public abstract class EventListenerNode : ListenerNode
    {
        public IFlowOutputField On { get; private set; }
        
        protected IGraphHubConnection GraphHubConnection { get; }

        protected EventListenerNode(IGraphHubConnection graphHubConnection)
        {
            GraphHubConnection = graphHubConnection;
        }

        protected override void Define()
        {
            On = AddFlowOutput(nameof(On));
        }

        public abstract override void SetupEventListeners();

        protected async Task SendErrorAsync(Exception ex)
        {
            await Console.Error.WriteLineAsync(ex.Message);
            await GraphHubConnection.SendGraphErrorAsync(new ExecutionErrorData
            {
                Bridge = Graph.Bridge,
                Message = ex.Message,
                GraphId = Graph.Id.ToString(),
                GraphName = Graph.Name,
                IsBuildError = false
            }, CancellationToken.None);
        }
    }
}