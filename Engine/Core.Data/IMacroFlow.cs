using Nodester.Graph.Core.Data.Fields;

namespace Nodester.Graph.Core.Data
{
    public interface IMacroFlow
    {
        void Run(IMacroFlowInputField start);
        IFlowOutputField Execute(IMacroFlowInputField start);
        T GetValue<T>(IMacroValueOutputField output);
    }
}