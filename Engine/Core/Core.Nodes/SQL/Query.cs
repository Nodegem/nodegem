using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Nodester.Common.Data;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.SQL
{
    public class Query : SQLQuery
    {
        [FieldAttributes("Query", ValueType.TextArea)]
        public IValueInputField QueryString { get; set; }
        
        protected override void Define()
        {
            base.Define();
            QueryString = AddValueInput(nameof(QueryString), default(string));
        }

        protected override async Task<IList<object>> RetrieveResultsAsync(IFlow flow)
        {
            var connectionString = await flow.GetValueAsync<string>(ConnectionString);
            var connection = new SqlConnection(connectionString);
            var query = await flow.GetValueAsync<string>(QueryString);
            var dapperResults = await connection.QueryAsync<dynamic>(query);
            var results = JsonConvert.DeserializeObject<IList<object>>(
                JsonConvert.SerializeObject(dapperResults));
            return results;
        }

        protected override async Task<IFlowOutputField> ExecuteSQLAsync(IFlow flow)
        {
            return await Task.FromResult(Out);
        }
    }
}