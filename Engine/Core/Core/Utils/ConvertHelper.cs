using System;

namespace Nodester.Graph.Core.Utils
{
    public class ConvertHelper
    {
        public static T Cast<T>(object value)
        {
            return (T) Convert.ChangeType(value, typeof(T));
        }
    }
}