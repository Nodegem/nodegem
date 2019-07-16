using System;
using System.Collections.Generic;
using Nodester.Engine.Data.Definitions;
using Nodester.Engine.Data.Fields;
using Nodester.Engine.Data.Links;

namespace Nodester.Engine.Data.Nodes
{
    public interface INode : IAsyncDisposable
    {
        Guid Id { get; }

        IGraph Graph { get; }
        
        IEnumerable<IFlowLink> FlowConnections { get; }
        IEnumerable<IValueLink> ValueConnections { get; }

        NodeDefinition GetDefinition();

        IField GetFieldByKey(string key);

        INode SetGraph(IGraph graph);

        INode SetId(Guid id);

        INode PopulateWithData(IEnumerable<FieldData> fields);

    }
}