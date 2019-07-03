using Nodester.Engine.Data.Links;

namespace Nodester.Engine.Data.Fields
{
    public interface IValueInputField : IValueField
    {
        object DefaultValue { get; }
        
        IValueLink Connection { get; }

        void SetConnection(IValueOutputField output);
    }
}