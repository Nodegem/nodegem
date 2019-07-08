using System;
using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Fields.Graph
{
    public class FlowInput : FlowField, IFlowInputField
    {
        public Func<IFlow, Task<IFlowOutputField>> Action { get; protected set; }

        public FlowInput(string key, Func<IFlow, Task<IFlowOutputField>> action) : base(key)
        {
            Action = action;
        }
    }
}