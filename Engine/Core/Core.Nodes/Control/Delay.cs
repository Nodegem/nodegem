using System;
using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using ValueType = Nodester.Engine.Data.ValueType;

namespace Nodester.Graph.Core.Nodes.Control
{
    [DefinedNode]
    [NodeNamespace("Core.Control")]
    public class Delay : Node
    {
        public IFlowInputField In { get; set; }
        public IFlowOutputField Out { get; set; }

        [FieldAttributes(ValueType.Number)]
        public IValueInputField DelayAmount { get; set; }

        protected override void Define()
        {
            In = AddFlowInput(nameof(In), PerformDelayAsync);
            Out = AddFlowOutput(nameof(Out));
            DelayAmount = AddValueInput<float>(nameof(DelayAmount));
        }

        private async Task<IFlowOutputField> PerformDelayAsync(IFlow flow)
        {
            var delayAmount = await flow.GetValueAsync<float>(DelayAmount);
            await Task.Delay(TimeSpan.FromMilliseconds(delayAmount));
            return Out;
        }
    }
}