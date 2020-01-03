using System;
using System.Threading.Tasks;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Exceptions;
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
            var currentNode = start.Node;
            try
            {
                var startConnection = start.Connection?.Destination;
                if (startConnection == null) return;

                currentNode = startConnection.Node;
                var flowOutput = await startConnection.Action(_flow);
                await _flow.RunAsync(flowOutput);
            }
            catch (ValueNodeException ex)
            {
                throw new MacroFlowException(ex, this, ex.Node);
            }
            catch (Exception ex)
            {
                throw new MacroFlowException(ex,this, currentNode);
            }
        }

        public async Task<IFlowOutputField> ExecuteAsync(IMacroFlowInputField start)
        {
            var currentNode = start.Node;
            try
            {
                var startConnection = start.Connection?.Destination;
                if (startConnection == null) return null;

                currentNode = startConnection.Node;
                var flowOutput = await startConnection.Action(_flow);
                while (flowOutput?.Connection?.Destination != null &&
                       !_macroGraph.IsMacroFlowOutputField(flowOutput.Key))
                {
                    currentNode = flowOutput.Connection.Destination.Node;
                    flowOutput = await flowOutput.Connection.Destination.Action(_flow);
                }

                return flowOutput;
            }
            catch (ValueNodeException ex)
            {
                throw new MacroFlowException(ex, this, ex.Node);
            }
            catch (Exception ex)
            {
                throw new MacroFlowException(ex, this, currentNode);
            }
        }

        public async Task<T> GetValueAsync<T>(IMacroValueOutputField output)
        {
            return (T) Convert.ChangeType(await output.Connection.Destination.GetValueAsync(_flow), typeof(T));
        }
    }
}