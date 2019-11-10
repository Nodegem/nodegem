using System;
using ValueType = Nodester.Common.Data.ValueType;

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