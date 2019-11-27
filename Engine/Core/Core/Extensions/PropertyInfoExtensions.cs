using System;
using System.Reflection;

namespace Nodegem.Engine.Core.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static T GetValue<T>(this PropertyInfo info, object obj)
        {
            try
            {
                var value = (T) info.GetValue(obj);
                return value;
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}