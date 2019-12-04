using System;
using System.Threading;
using System.Threading.Tasks;
using Nodegem.Common.Data;
using Nodegem.Common.Data.Interfaces;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Data.Nodes;

namespace Nodegem.Engine.Core
{
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