using System;
using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.Control
{
    [DefinedNode("Try / Catch")]
    [NodeNamespace("Core.Control")]
    public class TryCatch : Node
    {
        public FlowInput In { get; private set; }
        public FlowOutput Try { get; private set; }
        public FlowOutput Catch { get; private set; }
        
        [FieldAttributes("Error Message")]
        public ValueOutput ExceptionMessage { get; private set; }
        
        protected override void Define()
        {
            In = AddFlowInput(nameof(In), Execute);
            Try = AddFlowOutput(nameof(Try));
            Catch = AddFlowOutput(nameof(Catch));
            ExceptionMessage = AddValueOutput<string>(nameof(ExceptionMessage));
        }

        private async Task<IFlowOutputField> Execute(IFlow flow)
        {
            try
            {
                await flow.RunAsync(Try);
            }
            catch (Exception ex)
            {
                ExceptionMessage.SetValue(ex.Message);
                await flow.RunAsync(Catch);
            }

            return null;
        }
    }
}