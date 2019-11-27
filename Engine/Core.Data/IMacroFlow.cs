using System.Threading.Tasks;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Data
{
    public interface IMacroFlow
    {
        Task RunAsync(IMacroFlowInputField start);
        Task<IFlowOutputField> ExecuteAsync(IMacroFlowInputField start);
        Task<T> GetValueAsync<T>(IMacroValueOutputField output);
    }
}