using System;
using System.Threading.Tasks;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Macro
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

        public async Task RunAsync(IMacroFlowInputField start)
        {
            var startConnection = start.Connection?.Destination;
            if (startConnection == null)
            {
                return;
            }

            var flowOutput = await startConnection.Action(_flow);
            await _flow.RunAsync(flowOutput);
        }

        public async Task<IFlowOutputField> ExecuteAsync(IMacroFlowInputField start)
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

        public async Task<T> GetValueAsync<T>(IMacroValueOutputField output)
        {
            return (T) Convert.ChangeType(await output.Connection.Destination.GetValueAsync(_flow), typeof(T));
        }
    }
}