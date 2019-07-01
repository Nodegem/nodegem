using System;
using Nodester.Graph.Core.Data.Fields;

namespace Nodester.Graph.Core.Data
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