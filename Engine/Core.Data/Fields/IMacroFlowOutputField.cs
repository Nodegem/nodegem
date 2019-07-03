using System;

namespace Nodester.Engine.Data.Fields
{
    public interface IMacroFlowOutputField : IFlowInputField
    {
        void SetParentGraph(Func<IFlowGraph> getGraph);
    }
}