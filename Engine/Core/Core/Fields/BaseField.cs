using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Data.Nodes;

namespace Nodegem.Engine.Core.Fields
{
    public abstract class BaseField : IField
    {
        public INode Node { get; private set; }
        public string Key { get; }
        public string OriginalName { get; }

        protected BaseField(string key)
        {
            OriginalName = key;
            Key = key.ToLower();
        }

        protected BaseField(string key, INode node) : this(key)
        {
            Node = node;
        }

        public void SetNode(INode node)
        {
            Node = node;
        }
    }
}