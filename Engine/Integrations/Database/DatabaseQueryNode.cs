using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Nodegem.Common.Data;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Integrations.Database
{
    public abstract class DatabaseQueryNode : DatabaseNode
    {
        [FieldAttributes("Query", ValueType.TextArea)]
        public IValueInputField QueryString { get; set; }

        protected override void Define()
        {
            base.Define();
            QueryString = AddValueInput<string>(nameof(QueryString));
        }

        protected override async Task<object> ExecuteCommandAsync(IFlow flow)
        {
            var connectionString = await flow.GetValueAsync<string>(ConnectionString);
            var connection = CreateConnection(connectionString);
            var query = await flow.GetValueAsync<string>(QueryString);
            var dapperResults = await connection.QueryAsync<dynamic>(query);
            var results = JsonConvert.DeserializeObject<IList<object>>(
                JsonConvert.SerializeObject(dapperResults));
            return results;
        }
        
    }
}