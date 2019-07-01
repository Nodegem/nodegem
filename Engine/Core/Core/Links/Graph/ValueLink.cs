using Nodester.Graph.Core.Data.Fields;
using Nodester.Graph.Core.Data.Links;

namespace Nodester.Graph.Core.Links.Graph
{
    public class ValueLink : BaseLink<IValueOutputField, IValueInputField>, IValueLink
    {
        public ValueLink(IValueOutputField source, IValueInputField dest) : base(source, dest)
        {
        }
    }
}