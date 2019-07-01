using Nodester.Graph.Core.Data.Links;

namespace Nodester.Graph.Core.Data.Fields
{
    public interface IFlowOutputField : IFlowField
    {
        IFlowLink Connection { get; }

        void SetConnection(IFlowInputField input);
    }
}