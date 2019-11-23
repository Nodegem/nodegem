using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nodester.Common.Extensions;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using SqlKata.Execution;
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
        
        protected override void Define()
        {
            base.Define();
            Properties = InitializeValueInputList(nameof(Properties), "*");
            Table = AddValueInput(nameof(Table), default(string));
            Limit = AddValueInput(nameof(Limit), default(int?));
        }

        protected override async Task<IList<object>> RetrieveResultsAsync(IFlow flow)
        {
            var connectionString = await flow.GetValueAsync<string>(ConnectionString);
            var connection = new SqlConnection(connectionString);
            var properties = await Properties.SelectAsync(flow.GetValueAsync<string>);
            var db = CreateQueryFactory(connection);
            var table = await flow.GetValueAsync<string>(Table);
            var limit = await flow.GetValueAsync<int?>(Limit);
            var query = db.Query(table).Select(properties.ToArray());
            if (limit != null)
            {
                query.Limit(limit.Value);
            }

            //I need to find a more efficient way to do this
            var dapperResults = await query.GetAsync<dynamic>();
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