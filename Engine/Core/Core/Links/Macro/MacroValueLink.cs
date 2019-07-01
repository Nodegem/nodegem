using Nodester.Graph.Core.Data.Fields;
using Nodester.Graph.Core.Data.Links;

namespace Nodester.Graph.Core.Links.Macro
{
    public class MacroValueLink : BaseLink<IMacroValueOutputField, IValueOutputField>, IMacroValueLink
    {
        public MacroValueLink(IMacroValueOutputField source, IValueOutputField dest) : base(source, dest)
        {
        }
    }
}