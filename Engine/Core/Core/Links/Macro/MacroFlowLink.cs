using Nodester.Graph.Core.Data.Fields;
using Nodester.Graph.Core.Data.Links;

namespace Nodester.Graph.Core.Links.Macro
{
    public class MacroFlowLink : BaseLink<IMacroFlowInputField, IFlowInputField>, IMacroFlowLink
    {
        public MacroFlowLink(IMacroFlowInputField source, IFlowInputField dest) : base(source, dest)
        {
        }
    }
}