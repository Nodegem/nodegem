using Nodester.Graph.Core.Data.Links;

namespace Nodester.Graph.Core.Data.Fields
{
    public interface IValueInputField : IValueField
    {
        object DefaultValue { get; }
        
        IValueLink Connection { get; }

        void SetConnection(IValueOutputField output);
    }
}