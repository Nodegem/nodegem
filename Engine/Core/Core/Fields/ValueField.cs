using System;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Fields
{
    public class ValueField : BaseField, IValueField
    {
        public Type Type { get; }
        protected object Value { get; set; }

        protected ValueField(string key, Type type) : base(key)
        {
            Type = type;
        }

        public void SetValue(object value)
        {
            Value = value;
        }

        public virtual object GetValue()
        {
            return Value;
        }
    }
}