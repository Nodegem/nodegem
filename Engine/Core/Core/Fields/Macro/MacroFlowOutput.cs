using System;
using System.Threading.Tasks;
using Nodegem.Engine.Core.Fields.Graph;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Fields.Macro
{
    public class MacroFlowOutput : FlowInput, IMacroFlowOutputField
    {

        private Func<IFlowGraph> _getGraph; 
        
        public MacroFlowOutput(string key) : base(key, flow => Task.FromResult(default(IFlowOutputField)))
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