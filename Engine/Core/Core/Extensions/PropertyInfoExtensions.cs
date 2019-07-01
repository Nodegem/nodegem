using System.Reflection;

namespace Nodester.Graph.Core.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static T GetValue<T>(this PropertyInfo info, object obj)
        {
            return (T) info.GetValue(obj);
        }
    }
}