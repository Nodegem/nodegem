using System;
using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Fields;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Fields.Macro
{
    public class MacroFlowOutput : FlowInput, IMacroFlowOutputField
    {

        private Func<IFlowGraph> _getGraph; 
        
        public MacroFlowOutput(string key) : base(key, flow => null)
        {
        }

        public void SetParentGraph(Func<IFlowGraph> getGraph)
        {
            _getGraph = getGraph;
            Action = ContinueAction;
        }

        private async Task<IFlowOutputField> ContinueAction(IFlow flow)
        {
            var connection = _getGraph?.Invoke().GetConnection(Key)?.Destination;
            if (connection == null) return null;
            
            return await connection.Action(flow);
        }
    }
}