using System;
using ValueType = Nodegem.Common.Data.ValueType;

namespace Nodegem.Engine.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldAttributesAttribute : Attribute
    {
        public string Label { get; set; }
        public bool IsEditable { get; set; } = true;
        public bool AllowConnection { get; set; } = true;
        public string Info { get; set; }
        
        public object[] ValueOptions { get; set; }
        public Type EnumOptions { get; set; }

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