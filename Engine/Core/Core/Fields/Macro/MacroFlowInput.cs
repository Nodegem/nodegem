using Nodegem.Engine.Core.Links.Macro;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Data.Links;

namespace Nodegem.Engine.Core.Fields.Macro
{
    public class MacroFlowInput : FlowField, IMacroFlowInputField
    {
        public IMacroFlowLink Connection { get; private set; }

        public MacroFlowInput(string key) : base(key)
        {
        }

        public void SetConnection(IFlowInputField input)
        {
            Connection = new MacroFlowLink(this, input);
        }
    }
}