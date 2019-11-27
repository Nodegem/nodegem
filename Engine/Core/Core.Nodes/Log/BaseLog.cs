using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nodegem.Common.Data.Interfaces;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using ValueType = Nodegem.Common.Data.ValueType;

namespace Nodegem.Engine.Core.Nodes.Log
{
    [NodeNamespace("Core.Logging")]
    public abstract class BaseLog : Node
    {
        public IFlowInputField In { get; set; }
        public IFlowOutputField Out { get; set; }

        [FieldAttributes(ValueType.TextArea)] public IValueInputField Message { get; set; }

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
            var stringMessage = string.Empty;
            var message = await flow.GetValueAsync<object>(Message);

            stringMessage = !(message.GetType().IsPrimitive || message is JValue) ? JsonConvert.SerializeObject(message) : message.ToString();

            await ExecuteLogAsync(stringMessage, !Graph.IsRunningLocally);
            return Out;
        }

        protected abstract Task ExecuteLogAsync(string message, bool sendToClient);
    }
}