using System;

namespace Nodegem.Engine.Core.Utils
{
    public class ConvertHelper
    {
        
        public static object Cast(object value, Type type)
        {
            if (!(value is IConvertible))
            {
                return value;
            }

            return Convert.ChangeType(value, type);
        }
        
        public static T Cast<T>(object value)
        {
            if (!(value is IConvertible))
            {
                return (T) value;
            }

            return (T) Convert.ChangeType(value, typeof(T));
        }
    }
}