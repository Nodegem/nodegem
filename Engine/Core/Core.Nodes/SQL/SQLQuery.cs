using System.Collections.Generic;
using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.SQL
{
    public abstract class SQLQuery : SQLNode
    {
        public IValueOutputField Result { get; set; }

        protected override void Define()
        {
            base.Define();
            Result = AddValueOutput(nameof(Result), RetrieveResultsAsync);
        }

        protected abstract Task<IList<object>> RetrieveResultsAsync(IFlow flow);
    }
}