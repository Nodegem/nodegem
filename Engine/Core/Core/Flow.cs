using System;
using System.Collections.Generic;
using System.Linq;
using Nodester.Graph.Core.Data;
using Nodester.Graph.Core.Data.Fields;
using Nodester.Graph.Core.Fields.Graph;
using Nodester.Graph.Core.Utils;

namespace Nodester.Graph.Core
{
    public class Flow : IFlow
    {
        private Stack<Guid> LoopIds { get; }

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

        public void Run(IFlowOutputField output)
        {
            var connection = output.Connection;

            if (connection == null)
            {
                return;
            }

            var nextOutput = connection.Destination?.Action(this);
            while (nextOutput?.Connection != null)
            {
                nextOutput = nextOutput.Connection?.Destination?.Action(this);
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