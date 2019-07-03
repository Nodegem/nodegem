using Nodester.Engine.Data.Links;

namespace Nodester.Engine.Data.Fields
{
    public interface IFlowOutputField : IFlowField
    {
        IFlowLink Connection { get; }

        void SetConnection(IFlowInputField input);
    }
}