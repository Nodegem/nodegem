using System;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Fields;

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