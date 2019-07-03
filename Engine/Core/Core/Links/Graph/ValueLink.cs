using Nodester.Engine.Data.Fields;
using Nodester.Engine.Data.Links;

namespace Nodester.Graph.Core.Links.Graph
{
    public class ValueLink : BaseLink<IValueOutputField, IValueInputField>, IValueLink
    {
        public ValueLink(IValueOutputField source, IValueInputField dest) : base(source, dest)
        {
        }
    }
}