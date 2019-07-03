using Nodester.Engine.Data.Fields;

namespace Nodester.Engine.Data
{
    public interface IMacroFlow
    {
        void Run(IMacroFlowInputField start);
        IFlowOutputField Execute(IMacroFlowInputField start);
        T GetValue<T>(IMacroValueOutputField output);
    }
}