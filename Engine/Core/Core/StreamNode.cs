using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Data;
using Nodester.Data;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using Nodester.Engine.Data.Nodes;

namespace Nodester.Graph.Core
{
    [DefinedNode(IsListenerOnly = true)]
    public abstract class StreamNode : Node, IListenerNode
    {
        public new IListenerGraph Graph => base.Graph as IListenerGraph;
        
        public IFlowOutputField Out { get; private set; }
        
        protected IGraphHubConnection GraphHubConnection { get; }

        protected StreamNode(IGraphHubConnection graphHubConnection)
        {
            GraphHubConnection = graphHubConnection;
        }

        protected override void Define()
        {
            Out = AddFlowOutput(nameof(Out));
        }

        public abstract void SetupEventListeners();

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