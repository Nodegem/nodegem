using System;

namespace Nodegem.Engine.Data.Attributes
{
    public class FriendlyNameAttribute : Attribute
    {
        public string FriendlyName { get; }
        public object Value { get; set; }
        
        public FriendlyNameAttribute(string name)
        {
            FriendlyName = name;
        }
    }
}