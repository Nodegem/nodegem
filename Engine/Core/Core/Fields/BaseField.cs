using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Fields
{
    public abstract class BaseField : IField
    {
        public string Key { get; }
        public string OriginalName { get; }

        protected BaseField(string key)
        {
            OriginalName = key;
            Key = key.ToLower();
        }
    }
}