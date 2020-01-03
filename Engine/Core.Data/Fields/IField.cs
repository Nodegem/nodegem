using Nodegem.Engine.Data.Nodes;

namespace Nodegem.Engine.Data.Fields
{
    public interface IField
    {
        INode Node { get; }
        string Key { get; }
        string OriginalName { get; }
    }
}