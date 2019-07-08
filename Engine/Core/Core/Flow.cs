using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Fields;
using Nodester.Graph.Core.Fields.Graph;
using Nodester.Graph.Core.Utils;

namespace Nodester.Graph.Core
{
    public class Flow : IFlow
    {
        private Stack<Guid> LoopIds { get; }

        public bool IsRunningLocally { get; set; }

        private Guid CurrentLoop => LoopIds.Any() ? LoopIds.Peek() : Guid.Empty;

        public Flow()
        {
            LoopIds = new Stack<Guid>();
        }

        public Guid EnterLoop()
        {
            var loopId = Guid.NewGuid();
            LoopIds.Push(loopId);
            return loopId;
        }

        public bool HasLoopExited(Guid loopId)
        {
            return LoopIds.Any() && CurrentLoop == loopId;
        }

        public void BreakLoop()
        {
            if (CurrentLoop == Guid.Empty)
            {
                return;
            }

            LoopIds.Pop();
        }

        public void ExitLoop(Guid loopId)
        {
            if (CurrentLoop != loopId)
            {
                return;
            }

            LoopIds.Pop();
        }

        public async Task RunAsync(IFlowOutputField output)
        {
            var connection = output.Connection;

            var destination = connection?.Destination;
            if (destination == null) return;

            var nextOutput = await destination.Action(this);
            while (nextOutput?.Connection?.Destination != null)
            {
                nextOutput = await nextOutput.Connection?.Destination?.Action(this);
            }
        }

        private object GetValue(IValueInputField input)
        {
            var connection = input.Connection;
            return connection == null ? input.GetValue() : connection.Source.GetValue(this);
        }

        public T GetValue<T>(IValueInputField input)
        {
            return ConvertHelper.Cast<T>(GetValue(input));
        }
    }
}