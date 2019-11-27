using Nodegem.Engine.Data.Links;

namespace Nodegem.Engine.Data.Fields
{
    public interface IMacroValueOutputField : IValueField
    {
        IMacroValueLink Connection { get; }

        void SetConnection(IValueOutputField output);
    }
}