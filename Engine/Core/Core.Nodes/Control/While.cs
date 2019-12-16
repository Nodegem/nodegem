using System.Threading.Tasks;
using Nodegem.Common.Data;
using Nodegem.Engine.Core.Fields.Graph;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Control
{
    [DefinedNode("B37DDCBE-1E4D-4CAC-BDBE-6565CB4F43DE")]
    [NodeNamespace("Core.Control")]
    public class While : LoopNode
    {
        [FieldAttributes(ValueType.Boolean)]
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