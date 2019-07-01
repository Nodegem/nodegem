using System;
using Nodester.Graph.Core.Data;
using Nodester.Graph.Core.Data.Fields;

namespace Nodester.Graph.Core.Fields.Graph
{
    public class ValueOutput : ValueField, IValueOutputField
    {
        private Func<IFlow, object> ValueFunc { get; }

        public ValueOutput(string key, Func<IFlow, object> valueFunc, Type returnType) : base(key, returnType)
        {
            ValueFunc = valueFunc;
        }

        public ValueOutput(string key, Type returnType) : this(key, null, returnType)
        {
            ValueFunc = f => GetValue();
        }

        public object GetValue(IFlow flow)
        {
            return ValueFunc(flow);
        }
    }
}