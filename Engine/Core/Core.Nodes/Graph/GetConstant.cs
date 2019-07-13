using System;
using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
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
            Value = AddValueOutput<object>(ValueKey, GetValue);
        }

        private Task<object> GetValue(IFlow flow)
        {
            return Task.FromResult(Graph.GetConstant<object>(Key));
        }
    }
}