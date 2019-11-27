using System;

namespace Nodegem.Engine.Data.Fields
{
    public interface IMacroFlowOutputField : IFlowInputField
    {
        void SetParentGraph(Func<IFlowGraph> getGraph);
    }
}