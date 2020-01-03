using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nodegem.Engine.Core.Utils;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Exceptions;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Data.Nodes;

namespace Nodegem.Engine.Core
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
            var currentNode = output.Node;
            try
            {
                var destination = output.Connection?.Destination;
                if (destination == null) return;

                currentNode = destination.Node;
                var nextOutput = await destination.Action(this);
                while (nextOutput?.Connection?.Destination != null)
                {
                    currentNode = nextOutput.Connection.Destination.Node;
                    nextOutput = await nextOutput.Connection.Destination.Action(this);
                }
            }
            catch (ValueNodeException ex)
            {
                throw new FlowException(ex, this, ex.Node);
            }
            catch (Exception ex)
            {
                throw new FlowException(ex, this, currentNode);
            }
        }

        private async Task<object> GetValueAsync(IValueInputField input)
        {
            var connection = input.Connection;
            return connection == null ? input.GetValue() : await connection.Source.GetValueAsync(this);
        }

        public async Task<T> GetValueAsync<T>(IValueInputField input)
        {
            return ConvertHelper.Cast<T>(await GetValueAsync(input));
        }
    }
}