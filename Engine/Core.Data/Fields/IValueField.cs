using System;

namespace Nodester.Graph.Core.Data.Fields
{
    public interface IValueField : IField
    {
        Type Type { get; }

        void SetValue(object value);

        object GetValue();
    }
}