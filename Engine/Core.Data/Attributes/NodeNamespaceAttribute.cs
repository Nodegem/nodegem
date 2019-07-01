using System;

namespace Nodester.Graph.Core.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NodeNamespaceAttribute : Attribute
    {
        public string Namespace { get; }

        public NodeNamespaceAttribute(string @namespace)
        {
            Namespace = @namespace;
        }
    }
}