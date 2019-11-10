using System;

namespace Nodester.Graph.Core.Utils
{
    public class ConvertHelper
    {
        public static T Cast<T>(object value)
        {
            if (!(value is IConvertible) && typeof(T) == typeof(string))
            {
                return (T) (object) value.ToString();
            }
            
            if (!(value is IConvertible))
            {
                return (T) value;
            }

            return (T) Convert.ChangeType(value, typeof(T));
        }
    }
}