using Nodester.Engine.Data.Fields;
using Nodester.Engine.Data.Links;
using Nodester.Graph.Core.Links.Macro;

namespace Nodester.Graph.Core.Fields.Macro
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