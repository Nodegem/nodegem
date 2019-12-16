using System.Data.Common;
using System.Threading.Tasks;
using Nodegem.Engine.Core;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Integrations.Database
{
    public abstract class DatabaseNode : Node
    {
        public IValueInputField ConnectionString { get; set; }
        public IValueOutputField Result { get; set; }

        protected override void Define()
        {
            ConnectionString = AddValueInput<string>(nameof(ConnectionString));
            Result = AddValueOutput(nameof(Result), ExecuteCommandAsync);
        }
        
        protected abstract DbConnection CreateConnection(string connectionString);
        protected abstract Task<object> ExecuteCommandAsync(IFlow flow);
    }
}