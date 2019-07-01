using System;

namespace Nodester.Graph.Core.Data.Fields
{
    public interface IMacroFlowOutputField : IFlowInputField
    {
        void SetParentGraph(Func<IFlowGraph> getGraph);
    }
}