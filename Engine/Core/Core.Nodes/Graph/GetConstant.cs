using System;
using System.Threading.Tasks;
using Nodegem.Engine.Core.Fields.Graph;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;

namespace Nodegem.Engine.Core.Nodes.Graph
{
    [DefinedNode(ConstantDefinitionId, Title = "Get Constant", Ignore = true)]
    [NodeNamespace("Core.Graph")]
    public class GetConstant : Node
    {
        public const string ConstantDefinitionId = "7AEAB6DE-98FB-4617-B81C-C179D6438BC1";
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

        private Task<object> GetValue(IFlow flow)
        {
            return Task.FromResult(Graph.GetConstant<object>(Key));
        }
    }
}