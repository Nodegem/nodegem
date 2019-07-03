using Nodester.Engine.Data.Links;

namespace Nodester.Engine.Data.Fields
{
    public interface IMacroFlowInputField : IFlowField
    {
        IMacroFlowLink Connection { get; }

        void SetConnection(IFlowInputField input);
    }
}