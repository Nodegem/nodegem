using System.Threading.Tasks;
using Nodester.Engine.Data.Fields;

namespace Nodester.Engine.Data
{
    public interface IMacroFlow
    {
        Task RunAsync(IMacroFlowInputField start);
        Task<IFlowOutputField> ExecuteAsync(IMacroFlowInputField start);
        Task<T> GetValueAsync<T>(IMacroValueOutputField output);
    }
}