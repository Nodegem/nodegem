using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.Logic
{
    [DefinedNode]
    [NodeNamespace("Core.Logic")]
    public class If : Node
    {
        public FlowInput In { get; private set; }

        [FieldAttributes(Type = ValueType.Boolean)]
        public ValueInput Condition { get; private set; }

        public FlowOutput True { get; private set; }
        public FlowOutput False { get; private set; }

        protected override void Define()
        {
            In = AddFlowInput(nameof(In), CheckCondition);
            Condition = AddValueInput<bool>(nameof(Condition));
            True = AddFlowOutput(nameof(True));
            False = AddFlowOutput(nameof(False));
        }

        private FlowOutput CheckCondition(IFlow flow)
        {
            return flow.GetValue<bool>(Condition) ? True : False;
        }
    }
}