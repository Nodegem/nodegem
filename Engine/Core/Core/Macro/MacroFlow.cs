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

        public void Run(IMacroFlowInputField start)
        {
            var startConnection = start.Connection?.Destination;
            if (startConnection == null)
            {
                return;
            }

            var flowOutput = startConnection.Action(_flow);
            _flow.Run(flowOutput);
        }

        public IFlowOutputField Execute(IMacroFlowInputField start)
        {
            var startConnection = start.Connection?.Destination;
            if (startConnection == null)
            {
                return null;
            }

            var flowOutput = startConnection.Action(_flow);
            while (flowOutput != null && !_macroGraph.IsMacroFlowOutputField(flowOutput.Key))
            {
                flowOutput = flowOutput.Connection?.Destination?.Action(_flow);
            }

            return flowOutput;
        }

        public T GetValue<T>(IMacroValueOutputField output)
        {
            return (T) output.Connection.Destination.GetValue(_flow);
        }
    }
}