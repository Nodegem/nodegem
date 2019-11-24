using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Nodester.Common.Extensions;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using SqlKata.Compilers;
using ValueType = Nodester.Common.Data.ValueType;

namespace Nodester.Graph.Core.Nodes.SQL
{
    public class Select : SQLQuery
    {
        [FieldAttributes("Properties", ValueType.Text)]
        public IEnumerable<IValueInputField> Properties { get; set; }
        
        [FieldAttributes(ValueType.Text)]
        public IValueInputField Table { get; set; }
        
        [FieldAttributes(ValueType.Number)]
        public IValueInputField Limit { get; set; }
        
        [FieldAttributes(ValueType.TextArea)]
        public IValueInputField Where { get; set; }
        
        protected override void Define()
        {
            base.Define();
            Properties = InitializeValueInputList(nameof(Properties), "*");
            Table = AddValueInput(nameof(Table), default(string));
            Limit = AddValueInput(nameof(Limit), default(int));
            Where = AddValueInput(nameof(Where), default(string));
        }

        protected override async Task<IList<object>> RetrieveResultsAsync(IFlow flow)
        {
            var connectionString = await flow.GetValueAsync<string>(ConnectionString);
            var connection = new SqlConnection(connectionString);
            var properties = await Properties.SelectAsync(flow.GetValueAsync<string>);
            var compiler = new SqlServerCompiler();
            var table = await flow.GetValueAsync<string>(Table);
            var limit = await flow.GetValueAsync<int>(Limit) as int?;
            var where = await flow.GetValueAsync<string>(Where);
            var query = new SqlKata.Query(table).Select(properties.ToArray());
            if (limit.HasValue)
            {
                query.Limit(limit.Value);
            }

            if (!string.IsNullOrEmpty(where))
            {
                query = query.WhereRaw(where);
            }
            var sql = compiler.Compile(query);
            var dapperResults = await connection.QueryAsync(sql.Sql, sql.NamedBindings);
            
            //I need to find a more efficient way to do this
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