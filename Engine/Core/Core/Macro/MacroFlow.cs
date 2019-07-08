using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Macro
{
    public class MacroFlow : IMacroFlow
    {
        private readonly IFlow _flow;
        private readonly MacroGraph _macroGraph;

        public MacroFlow(MacroGraph macroGraph)
        {
            _macroGraph = macroGraph;
            _flow = new Flow();
        }

        public async Task RunAsync(IMacroFlowInputField start, bool isLocal = false)
        {
            _flow.IsRunningLocally = isLocal;
            var startConnection = start.Connection?.Destination;
            if (startConnection == null)
            {
                return;
            }

            var flowOutput = await startConnection.Action(_flow);
            await _flow.RunAsync(flowOutput);
        }

        public async Task<IFlowOutputField> Execute(IMacroFlowInputField start)
        {
            var startConnection = start.Connection?.Destination;
            if (startConnection == null)
            {
                return null;
            }

            var flowOutput = await startConnection.Action(_flow);
            while (flowOutput?.Connection?.Destination != null && !_macroGraph.IsMacroFlowOutputField(flowOutput.Key))
            {
                flowOutput = await flowOutput.Connection?.Destination?.Action(_flow);
            }

            return flowOutput;
        }

        public T GetValue<T>(IMacroValueOutputField output)
        {
            return (T) output.Connection.Destination.GetValue(_flow);
        }
    }
}