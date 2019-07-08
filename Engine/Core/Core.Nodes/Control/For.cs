using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.Control
{
    [DefinedNode]
    [NodeNamespace("Core.Control")]
    public class For : LoopNode
    {
        public ValueInput From { get; private set; }
        public ValueInput To { get; private set; }
        public ValueInput Step { get; private set; }
        public ValueOutput Index { get; private set; }

        protected override void Define()
        {
            From = AddValueInput<double>(nameof(From), 0);
            To = AddValueInput<double>(nameof(To), 10);
            Step = AddValueInput<double>(nameof(Step), 1);
            Index = AddValueOutput<double>(nameof(Index));

            base.Define();
        }

        private void MoveNext(IFlow flow, double step, ref double currentIndex)
        {
            currentIndex += step;
            Index.SetValue(currentIndex);
        }

        private bool CanMoveNext(double currentIndex, double lastIndex, bool isAscending)
        {
            if (isAscending)
            {
                return currentIndex < lastIndex;
            }

            return currentIndex > lastIndex;
        }

        protected override async Task<IFlowOutputField> OnLoop(IFlow flow)
        {
            var start = flow.GetValue<double>(From);
            var end = flow.GetValue<double>(To);
            var step = flow.GetValue<double>(Step);
            var ascending = start <= end;
            var currentIndex = start;
            Index.SetValue(currentIndex);

            var loopId = flow.EnterLoop();
            while (flow.HasLoopExited(loopId) && CanMoveNext(currentIndex, end, ascending))
            {
                await flow.RunAsync(Block);
                MoveNext(flow, step, ref currentIndex);
            }

            flow.ExitLoop(loopId);
            return Out;
        }
    }
}