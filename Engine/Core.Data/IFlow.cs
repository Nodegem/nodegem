using System;
using System.Threading.Tasks;
using Nodester.Engine.Data.Fields;

namespace Nodester.Engine.Data
{
    public interface IFlow
    {
        bool IsRunningLocally { get; set; }
        Guid EnterLoop();
        bool HasLoopExited(Guid loopId);
        void BreakLoop();
        void ExitLoop(Guid loopId);
        Task RunAsync(IFlowOutputField output);
        Task<T> GetValueAsync<T>(IValueInputField input);
    }
}