using System;
using Nodegem.Engine.Core.Utils;
using Nodegem.Engine.Data.Fields;
using ValueType = Nodegem.Common.Data.ValueType;

namespace Nodegem.Engine.Core.Fields
{
    public class ValueField : BaseField, IValueField
    {
        public Type Type { get; }
        public ValueType ValueType { get; }
        protected object Value { get; set; }

        protected ValueField(string key, Type type) : base(key)
        {
            Type = type;
            ValueType = GetValueTypeFromType();
        }

        public void SetValue(object value)
        {
            if (value == null) return;
            Value = ConvertHelper.Cast(value, Type);
        }

        public virtual object GetValue()
        {
            return Value;
        }

        private ValueType GetValueTypeFromType()
        {
            if (Type == typeof(int) || Type == typeof(double) || Type == typeof(float) || Type == typeof(decimal) ||
                Type == typeof(long) || Type == typeof(sbyte) || Type == typeof(short) || Type == typeof(uint) ||
                Type == typeof(ulong) || Type == typeof(ushort))
            {
                return ValueType.Number;
            }

            if (Type == typeof(DateTime))
            {
                return ValueType.DateTime;
            }

            if (Type == typeof(string) || Type == typeof(char))
            {
                return ValueType.Text;
            }

            return Type == typeof(bool) ? ValueType.Boolean : ValueType.Any;
        }
    }
}