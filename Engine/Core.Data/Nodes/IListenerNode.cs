namespace Nodegem.Engine.Data.Nodes
{
    public interface IListenerNode : INode
    {
        
        new IListenerGraph Graph { get; }
        void SetupEventListeners();

    }
}