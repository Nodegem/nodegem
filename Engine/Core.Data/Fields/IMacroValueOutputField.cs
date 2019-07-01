using Nodester.Graph.Core.Data.Links;

namespace Nodester.Graph.Core.Data.Fields
{
    public interface IMacroValueOutputField : IValueField
    {
        IMacroValueLink Connection { get; }

        void SetConnection(IValueOutputField output);
    }
}