using Nodegem.Engine.Data.Links;

namespace Nodegem.Engine.Data.Fields
{
    public interface IValueInputField : IValueField
    {
        object DefaultValue { get; }
        
        IValueLink Connection { get; }

        void SetConnection(IValueOutputField output);
    }
}