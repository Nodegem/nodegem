using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nodester.Graph.Core.Utils
{
    public class ConvertHelper
    {
        public static T Cast<T>(object value)
        {
            if (!(value is IConvertible) && typeof(T) == typeof(string))
            {
                if (value is JObject jObject)
                {
                    return (T) (object) jObject.ToString(Formatting.None);
                }
                
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