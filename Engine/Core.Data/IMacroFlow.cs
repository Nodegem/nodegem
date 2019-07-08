using System.Threading.Tasks;
using Nodester.Engine.Data.Fields;

namespace Nodester.Engine.Data
{
    public interface IMacroFlow
    {
        Task RunAsync(IMacroFlowInputField start, bool isLocal = false);
        Task<IFlowOutputField> Execute(IMacroFlowInputField start);
        T GetValue<T>(IMacroValueOutputField output);
    }
}