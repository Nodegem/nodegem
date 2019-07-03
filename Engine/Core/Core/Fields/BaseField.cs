using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Fields
{
    public abstract class BaseField : IField
    {
        public string Key { get; }

        protected BaseField(string key)
        {
            Key = key.ToLower();
        }
    }
}