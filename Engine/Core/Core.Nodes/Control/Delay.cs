using System;
using System.Threading.Tasks;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using ValueType = Nodegem.Common.Data.ValueType;

namespace Nodegem.Engine.Core.Nodes.Control
{
    [DefinedNode("167A1E87-899C-4301-8406-1E70BCBE9EA9")]
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