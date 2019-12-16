using System;
using System.Threading.Tasks;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Data
{
    public interface IFlow
    {
        Guid EnterLoop();
        bool HasLoopExited(Guid loopId);
        void BreakLoop();
        void ExitLoop(Guid loopId);
        Task RunAsync(IFlowOutputField output);
        Task<T> GetValueAsync<T>(IValueInputField input);
    }
}