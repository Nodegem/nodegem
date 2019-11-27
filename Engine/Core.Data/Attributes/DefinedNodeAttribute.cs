using System;

namespace Nodegem.Engine.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DefinedNodeAttribute : Attribute
    {
        public string Id { get; }
        public string Title { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Ignore the node aggregator which is sent out to clients
        /// </summary>
        public bool Ignore { get; set; }
        public bool IgnoreDisplay { get; set; }
        public bool IsListenerOnly { get; set; }

        public DefinedNodeAttribute(string id)
        {
            Id = id;
        }

        public DefinedNodeAttribute(bool isListenerOnly)
        {
            IsListenerOnly = isListenerOnly;
        }

    }
}