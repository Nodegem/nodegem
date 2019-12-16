using Nodegem.Engine.Data.Links;

namespace Nodegem.Engine.Data.Fields
{
    public interface IFlowOutputField : IFlowField
    {
        IFlowLink Connection { get; }

        void SetConnection(IFlowInputField input);
    }
}