using System;
using Nodester.Engine.Data.Fields;

namespace Nodester.Engine.Data
{
    public interface IFlow
    {
        Guid EnterLoop();
        bool HasLoopExited(Guid loopId);
        void BreakLoop();
        void ExitLoop(Guid loopId);
        void Run(IFlowOutputField output);
        T GetValue<T>(IValueInputField input);
    }
}