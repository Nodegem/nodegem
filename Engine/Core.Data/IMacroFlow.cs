using System.Threading.Tasks;
using Nodester.Engine.Data.Fields;

namespace Nodester.Engine.Data
{
    public interface IMacroFlow
    {
        Task RunAsync(IMacroFlowInputField start, bool isLocal = true);
        Task<IFlowOutputField> Execute(IMacroFlowInputField start, bool isLocal = true);
        T GetValue<T>(IMacroValueOutputField output);
    }
}