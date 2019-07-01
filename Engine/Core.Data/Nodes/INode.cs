using System;
using System.Collections;
using System.Collections.Generic;
using Nodester.Graph.Core.Data.Definitions;
using Nodester.Graph.Core.Data.Fields;
using Nodester.Graph.Core.Data.Links;

namespace Nodester.Graph.Core.Data.Nodes
{
    public interface INode
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