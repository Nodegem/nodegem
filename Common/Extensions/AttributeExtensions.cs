using System;
using System.Linq;

namespace Nodegem.Common.Extensions
{
    public static class AttributeExtensions
    {
        public static TValue GetAttributeValue<TAttribute, TValue>(this Type type,
            Func<TAttribute, TValue> valueSelector, TValue @default = default(TValue))
            where TAttribute : Attribute
        {
            return type.GetCustomAttributes(typeof(TAttribute), true)
                .FirstOrDefault() is TAttribute att ? valueSelector(att) : @default;
        }
    }
}