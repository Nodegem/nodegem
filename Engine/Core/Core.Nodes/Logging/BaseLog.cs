using Nodester.Common.Data.Interfaces;
using Nodester.Graph.Core.Data;
using Nodester.Graph.Core.Data.Attributes;
using Nodester.Graph.Core.Data.Fields;
using Nodester.Graph.Core.Fields.Graph;
using ValueInput = Nodester.Graph.Core.Fields.Graph.ValueInput;

namespace Nodester.Graph.Core.Nodes.Logging
{
    public abstract class BaseLog : Node
    {
        public FlowInput In { get; private set; }
        public FlowOutput Out { get; private set; }

        [FieldAttributes(Type = ValueType.TextArea)]
        public ValueInput Message { get; private set; }

        protected ILogService LogService;

        protected BaseLog(ILogService logService)
        {
            LogService = logService;
        }

        protected override void Define()
        {
            In = AddFlowInput(nameof(In), PerformLog);
            Out = AddFlowOutput(nameof(Out));
            Message = AddValueInput(nameof(Message), "");
        }

        private IFlowOutputField PerformLog(IFlow flow)
        {
            ExecuteLog(flow.GetValue<string>(Message));
            return Out;
        }

        protected abstract void ExecuteLog(string message);
    }
}