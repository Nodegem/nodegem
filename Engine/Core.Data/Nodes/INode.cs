using System;
using System.Collections.Generic;
using Nodegem.Common.Data;
using Nodegem.Engine.Data.Definitions;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Data.Links;

namespace Nodegem.Engine.Data.Nodes
{
    public interface INode : IAsyncDisposable
    {
        Guid Id { get; }
        string Title { get; }

        IGraph Graph { get; }
        
        IEnumerable<IFlowLink> FlowConnections { get; }
        IEnumerable<IValueLink> ValueConnections { get; }

        NodeDefinition GetDefinition();

        IField GetFieldByKey(string key);

        INode SetGraph(IGraph graph);

        INode SetId(Guid id);

        INode PopulateWithData(IEnumerable<FieldData> fields);

        INode PopulateIndefinites(IEnumerable<KeyValuePair<string, string>> indefiniteKeyValuePairs);

    }
}