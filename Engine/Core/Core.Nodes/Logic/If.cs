using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.Logic
{
    [DefinedNode]
    [NodeNamespace("Core.Logic")]
    public class If : Node
    {
        public IFlowInputField In { get; private set; }

        [FieldAttributes(Type = ValueType.Boolean)]
        public IValueInputField Condition { get; private set; }

        public IFlowOutputField True { get; private set; }
        public IFlowOutputField False { get; private set; }

        protected override void Define()
        {
            In = AddFlowInput(nameof(In), CheckCondition);
            Condition = AddValueInput<bool>(nameof(Condition));
            True = AddFlowOutput(nameof(True));
            False = AddFlowOutput(nameof(False));
        }

        private Task<IFlowOutputField> CheckCondition(IFlow flow)
        {
            return Task.FromResult(flow.GetValue<bool>(Condition) ? True : False);
        }
    }
}