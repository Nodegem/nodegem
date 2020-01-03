using System;
using System.Threading.Tasks;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Data.Nodes;

namespace Nodegem.Engine.Core.Fields.Graph
{
    public class FlowInput : FlowField, IFlowInputField
    {
        public Func<IFlow, Task<IFlowOutputField>> Action { get; protected set; }

        public FlowInput(string key, Func<IFlow, Task<IFlowOutputField>> action) : base(key)
        {
            Action = action;
        }

        public FlowInput(string key, Func<IFlow, Task<IFlowOutputField>> action, INode node) : base(key, node)
        {
            Action = action;
        }
    }
}