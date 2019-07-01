using System;
using Nodester.Graph.Core.Data;
using Nodester.Graph.Core.Data.Attributes;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.Graph
{
    [DefinedNode("Get Constant", Ignore = true)]
    [NodeNamespace("Core.Graph")]
    public class GetConstant : Node
    {

        public const string ValueKey = nameof(Value);
        
        private Guid Key { get; }
        public ValueOutput Value { get; private set; }

        public GetConstant(Guid constantKey)
        {
            Key = constantKey;
        }

        protected override void Define()
        {
            Value = AddValueOutput(ValueKey, GetValue);
        }

        private object GetValue(IFlow flow)
        {
            return Graph.GetConstant<object>(Key);
        }
    }
}