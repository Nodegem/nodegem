using System;

namespace Nodester.Graph.Core.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DefinedNodeAttribute : Attribute
    {
        public string Title { get; }
        public string Description { get; set; }

        /// <summary>
        /// Ignore the node aggregator which is sent out to clients
        /// </summary>
        public bool Ignore { get; set; }

        public DefinedNodeAttribute()
        {
        }

        public DefinedNodeAttribute(string title)
        {
            Title = title;
        }

        public DefinedNodeAttribute(string title, string description) : this(title)
        {
            Description = description;
        }
    }
}