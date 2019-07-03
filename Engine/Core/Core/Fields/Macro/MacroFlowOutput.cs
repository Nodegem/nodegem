using System;
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

        private IFlowOutputField ContinueAction(IFlow flow)
        {
            return _getGraph?.Invoke().GetConnection(Key)?.Destination?.Action(flow);
        }
    }
}