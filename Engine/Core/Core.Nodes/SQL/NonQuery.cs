using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.SQL
{
    public abstract class NonQuery : SQLNode
    {
        protected override Task<IFlowOutputField> ExecuteSQLAsync(IFlow flow)
        {
            throw new System.NotImplementedException();
        }
    }
}