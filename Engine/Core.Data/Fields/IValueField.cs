using System;

namespace Nodester.Engine.Data.Fields
{
    public interface IValueField : IField
    {
        Type Type { get; }
        
        ValueType ValueType { get; }

        void SetValue(object value);

        object GetValue();
    }
}