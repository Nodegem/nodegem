using System;
using System.Threading.Tasks;
using Nodegem.Engine.Core.Fields.Graph;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Control
{
    [DefinedNode("CCF5971F-C627-4522-9BAB-2D83039B49D7", Title = "Try / Catch")]
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