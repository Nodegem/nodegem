using System.Threading.Tasks;
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

        private async Task<bool> CanMoveNext(IFlow flow)
        {
            return await flow.GetValueAsync<bool>(Condition);
        }

        protected override async Task<IFlowOutputField> OnLoop(IFlow flow)
        {
            var loopId = flow.EnterLoop();

            while (flow.HasLoopExited(loopId) && await CanMoveNext(flow))
            {
                await flow.RunAsync(Block);
            }

            flow.ExitLoop(loopId);
            return Out;
        }
    }
}