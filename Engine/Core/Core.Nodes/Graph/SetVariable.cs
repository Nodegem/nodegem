using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.Graph
{
    [DefinedNode("Set Variable")]
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
            Value = AddValueOutput(nameof(Value), GetValue);
        }

        private Task<IFlowOutputField> SetValue(IFlow flow)
        {
            Graph.SetVariable(flow.GetValue<string>(Variable), flow.GetValue<object>(NewValue));
            return Task.FromResult(Out);
        }

        private object GetValue(IFlow flow)
        {
            return Graph.GetVariable<object>(flow.GetValue<string>(Variable));
        }
    }
}