using System;

namespace Nodester.Engine.Data.Nodes
{
    public interface IListenerNode : INode, IAsyncDisposable
    {
        
        new IListenerGraph Graph { get; }

        void SetupEventListeners();

    }
}