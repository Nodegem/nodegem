using System;

namespace Nodester.Graph.Core.Data.Fields
{
    public interface IValueOutputField : IValueField
    {
        object GetValue(IFlow flow);
    }
}