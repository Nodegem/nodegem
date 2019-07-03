using Nodester.Engine.Data.Fields;
using Nodester.Engine.Data.Links;

namespace Nodester.Graph.Core.Links.Macro
{
    public class MacroFlowLink : BaseLink<IMacroFlowInputField, IFlowInputField>, IMacroFlowLink
    {
        public MacroFlowLink(IMacroFlowInputField source, IFlowInputField dest) : base(source, dest)
        {
        }
    }
}