using System;
using System.Linq;

namespace Nodester.Common.Extensions
{
    public static class AttributeExtensions
    {
        public static TValue GetAttributeValue<TAttribute, TValue>(this Type type,
            Func<TAttribute, TValue> valueSelector, TValue @default = default(TValue))
            where TAttribute : Attribute
        {
            var att = type.GetCustomAttributes(typeof(TAttribute), true)
                .FirstOrDefault() as TAttribute;

            return att != null ? valueSelector(att) : @default;
        }
    }
}