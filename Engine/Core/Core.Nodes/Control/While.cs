using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.Control
{
    [DefinedNode]
    [NodeNamespace("Core.Control")]
    public class While : LoopNode
    {
        [FieldAttributes(Type = ValueType.Boolean)]
        public ValueInput Condition { get; private set; }

        protected override void Define()
        {
            Condition = AddValueInput<bool>(nameof(Condition));
            base.Define();
        }

        private bool CanMoveNext(IFlow flow)
        {
            return flow.GetValue<bool>(Condition);
        }

        protected override IFlowOutputField OnLoop(IFlow flow)
        {
            var loopId = flow.EnterLoop();

            while (flow.HasLoopExited(loopId) && CanMoveNext(flow))
            {
                flow.Run(Block);
            }

            flow.ExitLoop(loopId);
            return Out;
        }
    }
}