using System;
using System.Threading.Tasks;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Exceptions;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Data.Nodes;

namespace Nodegem.Engine.Core.Fields.Graph
{
    public class ValueOutput : ValueField, IValueOutputField
    {
        private Func<IFlow, Task<object>> ValueFunc { get; }

        public ValueOutput(string key, Func<IFlow, Task<object>> valueFunc, Type returnType) : base(key,
            returnType)
        {
            ValueFunc = valueFunc;
        }

        public ValueOutput(string key, Func<IFlow, Task<object>> valueFunc, Type returnType, INode node) : base(key,
            returnType, node)
        {
            ValueFunc = valueFunc;
        }

        public ValueOutput(string key, Type returnType, INode node) : this(key, null, returnType, node)
        {
            ValueFunc = f => Task.FromResult(GetValue());
        }

        public ValueOutput(string key, Type returnType) : this(key, null, returnType)
        {
            ValueFunc = f => Task.FromResult(GetValue());
        }

        public async Task<object> GetValueAsync(IFlow flow)
        {
            try
            {
                return await ValueFunc(flow);
            }
            catch (Exception ex)
            {
                throw new ValueNodeException(ex, Node);
            }
        }
    }
}