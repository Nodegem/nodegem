using System.Collections.Generic;
using System.Linq;

namespace Nodester.Common.Utilities
{
    public abstract class HierarchicalNode<TItem, THierarchicalNode>
        where THierarchicalNode : HierarchicalNode<TItem, THierarchicalNode>, new()
    {
        protected HierarchicalNode()
        {
            Children = new Dictionary<string, THierarchicalNode>();
            Items = new List<TItem>();
        }

        public IDictionary<string, THierarchicalNode> Children { get; }

        public ICollection<TItem> Items { get; }
        
        public THierarchicalNode GetOrAddChild(string namespaceName)
        {
            THierarchicalNode child;
            if (!Children.TryGetValue(namespaceName, out child))
                child = Children[namespaceName] = new THierarchicalNode();
            return child;
        }

        public void AddObject(IEnumerable<string> nodeNames, TItem item)
        {
            AddObject(nodeNames, 0, item);
        }

        private void AddObject(IEnumerable<string> nodeNames, int index, TItem item)
        {
            var nodeList = nodeNames.ToList();
            if (index >= nodeList.Count())
                Items.Add(item);
            else
            {
                GetOrAddChild(nodeList[index]).AddObject(nodeList, index + 1, item);
            }
        }
    }
}