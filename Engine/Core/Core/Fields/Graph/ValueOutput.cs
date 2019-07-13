using System;
using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Fields.Graph
{
    public class ValueOutput : ValueField, IValueOutputField
    {
        private Func<IFlow, Task<object>> ValueFunc { get; }

        public ValueOutput(string key, Func<IFlow, Task<object>> valueFunc, Type returnType) : base(key, returnType)
        {
            ValueFunc = valueFunc;
        }

        public ValueOutput(string key, Type returnType) : this(key, null, returnType)
        {
            ValueFunc = f => Task.FromResult(GetValue());
        }

        public async Task<object> GetValueAsync(IFlow flow)
        {
            return await ValueFunc(flow);
        }
    }
}