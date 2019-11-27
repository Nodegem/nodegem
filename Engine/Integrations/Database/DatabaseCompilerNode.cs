using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Nodegem.Common.Data;
using Nodegem.Common.Extensions;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using SqlKata.Compilers;

namespace Nodegem.Engine.Integrations.Database
{
    public abstract class DatabaseCompilerNode : DatabaseNode
    {
        
        [FieldAttributes("Properties", ValueType.Text)]
        public IEnumerable<IValueInputField> Properties { get; set; }
        
        [FieldAttributes(ValueType.Text)]
        public IValueInputField Table { get; set; }
        
        [FieldAttributes(ValueType.Number)]
        public IValueInputField Limit { get; set; }
        
        [FieldAttributes(ValueType.TextArea)]
        public IValueInputField Where { get; set; }

        protected abstract Compiler BuildCompiler();

        protected override void Define()
        {
            base.Define();
            Properties = InitializeValueInputList(nameof(Properties), "*");
            Table = AddValueInput(nameof(Table), default(string));
            Limit = AddValueInput(nameof(Limit), default(int));
            Where = AddValueInput(nameof(Where), default(string));
        }

        protected override async Task<object> ExecuteCommandAsync(IFlow flow)
        {
            var connectionString = await flow.GetValueAsync<string>(ConnectionString);
            var connection = CreateConnection(connectionString);
            var properties = await Properties.SelectAsync(flow.GetValueAsync<string>);
            var compiler = BuildCompiler();
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
    }
}