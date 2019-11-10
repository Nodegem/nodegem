using System;
using ValueType = Nodester.Common.Data.ValueType;

namespace Nodester.Engine.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldAttributesAttribute : Attribute
    {
        public string Label { get; set; }
        public bool IsEditable { get; set; } = true;

        private string _key;

        public string Key
        {
            get => _key;
            set => _key = value.ToLower();
        }
        public ValueType? Type { get; set; }
        
        public FieldAttributesAttribute()
        {
        }

        public FieldAttributesAttribute(string label)
        {
            Label = label;
        }

        public FieldAttributesAttribute(ValueType type)
        {
            Type = type;
        }

        public FieldAttributesAttribute(string label, ValueType type) : this(label)
        {
            Type = type;
        }
    }
}