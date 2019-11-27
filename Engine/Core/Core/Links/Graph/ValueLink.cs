using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Data.Links;

namespace Nodegem.Engine.Core.Links.Graph
{
    public class ValueLink : BaseLink<IValueOutputField, IValueInputField>, IValueLink
    {
        public ValueLink(IValueOutputField source, IValueInputField dest) : base(source, dest)
        {
        }
    }
}