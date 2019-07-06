using Nodester.Engine.Data.Fields;

namespace Nodester.Engine.Data
{
    public interface IMacroFlow
    {
        void Run(IMacroFlowInputField start, bool isLocal = false);
        IFlowOutputField Execute(IMacroFlowInputField start);
        T GetValue<T>(IMacroValueOutputField output);
    }
}