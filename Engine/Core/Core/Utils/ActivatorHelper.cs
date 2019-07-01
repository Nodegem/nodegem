using System;

namespace Nodester.Graph.Core.Utils
{
    public static class ActivatorHelper
    {
        public static object CreateInstance(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type == null) return default(object);

            return Activator.CreateInstance(type);
        }
    }
}