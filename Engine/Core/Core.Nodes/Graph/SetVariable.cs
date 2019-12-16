using System.Threading.Tasks;
using Nodegem.Engine.Core.Fields.Graph;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Graph
{
    [DefinedNode("3DBF95B1-D792-4A11-8AC4-8A0105AFD03F", Title = "Set Variable")]
    [NodeNamespace("Core.Graph")]
    public class SetVariable : Node
    {
        public IFlowInputField In { get; private set; }
        public IFlowOutputField Out { get; private set; }
        public IValueInputField Variable { get; private set; }

        [FieldAttributes("New Value")] public ValueInput NewValue { get; private set; }

        public ValueOutput Value { get; private set; }

        protected override void Define()
        {
            In = AddFlowInput(nameof(In), SetValue);
            Out = AddFlowOutput(nameof(Out));

            Variable = AddValueInput<string>(nameof(Variable));
            NewValue = AddValueInput<object>(nameof(NewValue));
            Value = AddValueOutput<object>(nameof(Value), GetValue);
        }

        private async Task<IFlowOutputField> SetValue(IFlow flow)
        {
            Graph.SetVariable(await flow.GetValueAsync<string>(Variable), flow.GetValueAsync<object>(NewValue));
            return Out;
        }

        private async Task<object> GetValue(IFlow flow)
        {
            return Graph.GetVariable<object>(await flow.GetValueAsync<string>(Variable));
        }
    }
}