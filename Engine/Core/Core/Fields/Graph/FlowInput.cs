using System;
using Nodester.Graph.Core.Data;
using Nodester.Graph.Core.Data.Fields;

namespace Nodester.Graph.Core.Fields.Graph
{
    public class FlowInput : FlowField, IFlowInputField
    {
        public Func<IFlow, IFlowOutputField> Action { get; protected set; }

        public FlowInput(string key, Func<IFlow, IFlowOutputField> action) : base(key)
        {
            Action = action;
        }
    }
}