using Nodester.Graph.Core.Data.Links;

namespace Nodester.Graph.Core.Data.Fields
{
    public interface IMacroFlowInputField : IFlowField
    {
        IMacroFlowLink Connection { get; }

        void SetConnection(IFlowInputField input);
    }
}