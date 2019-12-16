using Nodegem.Engine.Data.Links;

namespace Nodegem.Engine.Data.Fields
{
    public interface IMacroFlowInputField : IFlowField
    {
        IMacroFlowLink Connection { get; }

        void SetConnection(IFlowInputField input);
    }
}