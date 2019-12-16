using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Data.Links;

namespace Nodegem.Engine.Core.Links.Macro
{
    public class MacroFlowLink : BaseLink<IMacroFlowInputField, IFlowInputField>, IMacroFlowLink
    {
        public MacroFlowLink(IMacroFlowInputField source, IFlowInputField dest) : base(source, dest)
        {
        }
    }
}