using System.Threading.Tasks;
using Newtonsoft.Json;
using Nodegem.Common.Data;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Exceptions;
using Nodegem.Engine.Data.Fields;
using Nodegem.Services.Data;

namespace Nodegem.Engine.Integrations.ApplicationInsights
{
    [DefinedNode("10E6C2D0-D72A-4FB3-BC54-6A9D808B93F3")]
    public class Query : AppInsightNode
    {
        [FieldAttributes(ValueType.TextArea)] public IValueInputField QueryString { get; set; }

        public Query(INodeHttpClient client) : base(client)
        {
        }

        protected override void Define()
        {
            base.Define();
            QueryString = AddValueInput<string>("Query");
        }

        protected override async Task<object> GetResultsAsync(IFlow flow)
        {
            var appId = await flow.GetValueAsync<string>(AppId);
            var apiKey = await flow.GetValueAsync<string>(ApiKey);
            var query = await flow.GetValueAsync<string>(QueryString);
            var url = BuildUrl(appId, query);
            var client = BuildClient(apiKey);
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode) throw new GraphException(response.ReasonPhrase, Graph);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject(json);
        }
    }
}