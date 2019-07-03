using Nodester.Engine.Data.Links;

namespace Nodester.Engine.Data.Fields
{
    public interface IMacroValueOutputField : IValueField
    {
        IMacroValueLink Connection { get; }

        void SetConnection(IValueOutputField output);
    }
}