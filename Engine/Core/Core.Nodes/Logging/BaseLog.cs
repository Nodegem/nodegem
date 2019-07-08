using System.Threading.Tasks;
using Nodester.Common.Data.Interfaces;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using ValueInput = Nodester.Graph.Core.Fields.Graph.ValueInput;

namespace Nodester.Graph.Core.Nodes.Logging
{
    public abstract class BaseLog : Node
    {
        public IFlowInputField In { get; private set; }
        public IFlowOutputField Out { get; private set; }

        [FieldAttributes(Type = ValueType.TextArea)]
        public IValueInputField Message { get; private set; }

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

        private Task<IFlowOutputField> PerformLog(IFlow flow)
        {
            ExecuteLog(flow.GetValue<string>(Message), !flow.IsRunningLocally);
            return Task.FromResult(Out);
        }

        protected abstract void ExecuteLog(string message, bool sendToClient);
    }
}