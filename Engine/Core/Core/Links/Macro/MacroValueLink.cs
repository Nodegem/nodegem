using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Data.Links;

namespace Nodegem.Engine.Core.Links.Macro
{
    public class MacroValueLink : BaseLink<IMacroValueOutputField, IValueOutputField>, IMacroValueLink
    {
        public MacroValueLink(IMacroValueOutputField source, IValueOutputField dest) : base(source, dest)
        {
        }
    }
}