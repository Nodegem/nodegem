using System;

namespace Nodester.Engine.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldAttributesAttribute : Attribute
    {
        public string Label { get; set; }
        public ValueType Type { get; set; } = ValueType.Any;

        public FieldAttributesAttribute()
        {
        }

        public FieldAttributesAttribute(string label)
        {
            Label = label;
        }

        public FieldAttributesAttribute(string label, ValueType type) : this(label)
        {
            Type = type;
        }
    }
}