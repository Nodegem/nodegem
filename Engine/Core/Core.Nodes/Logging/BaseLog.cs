using System.Threading.Tasks;
using Nodester.Common.Data.Interfaces;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.Logging
{
    [DefinedNode]
    [NodeNamespace("Core.Logging")]
    public abstract class BaseLog : Node
    {
        public IFlowInputField In { get; set; }
        public IFlowOutputField Out { get; set; }

        [FieldAttributes(Type = ValueType.TextArea)]
        public IValueInputField Message { get; set; }

        protected ITerminalHubService LogService { get; }

        protected BaseLog(ITerminalHubService logService)
        {
            LogService = logService;
        }

        protected override void Define()
        {
            In = AddFlowInput(nameof(In), PerformLog);
            Out = AddFlowOutput(nameof(Out));
            Message = AddValueInput(nameof(Message), "");
        }

        private async Task<IFlowOutputField> PerformLog(IFlow flow)
        {
            ExecuteLog(await flow.GetValueAsync<string>(Message), !Graph.IsRunningLocally);
            return Out;
        }

        protected abstract void ExecuteLog(string message, bool sendToClient);
    }
}