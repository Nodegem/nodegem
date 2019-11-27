using System.Threading.Tasks;
using Nodegem.Common.Data;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Logic
{
    [DefinedNode("92F804B7-9C8C-4184-817F-266C316673E3")]
    [NodeNamespace("Core.Logic")]
    public class If : Node
    {
        public IFlowInputField In { get; private set; }

        [FieldAttributes(ValueType.Boolean)]
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

        private async Task<IFlowOutputField> CheckCondition(IFlow flow)
        {
            return await flow.GetValueAsync<bool>(Condition) ? True : False;
        }
    }
}