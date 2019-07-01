using System;
using Nodester.Graph.Core.Data;
using Nodester.Graph.Core.Data.Attributes;
using Nodester.Graph.Core.Data.Fields;
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

        private IFlowOutputField Execute(IFlow flow)
        {
            try
            {
                flow.Run(Try);
            }
            catch (Exception ex)
            {
                ExceptionMessage.SetValue(ex.Message);
                flow.Run(Catch);
            }

            return null;
        }
    }
}