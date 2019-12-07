using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Nodegem.Common.Data;
using Nodegem.Engine.Core;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using Nodegem.Services.Data;

namespace Nodegem.Engine.Integrations.ApplicationInsights
{
    [NodeNamespace("Integrations.AppInsights")]
    public abstract class AppInsightNode : Node
    {
        protected const string AppInsightUrl = "https://api.applicationinsights.io/v1/apps/{0}/query?query={1}";
        
        [FieldAttributes(ValueType.Text)]
        public IValueInputField ApiKey { get; set; }
        
        [FieldAttributes(ValueType.Text)]
        public IValueInputField AppId { get; set; }
        
        public IValueOutputField Results { get; set; }
        
        private INodeHttpClient NodeClient { get; }

        protected AppInsightNode(INodeHttpClient client)
        {
            NodeClient = client;
        }

        protected override void Define()
        {
            ApiKey = AddValueInput<string>(nameof(ApiKey));
            AppId = AddValueInput<string>(nameof(AppId));
            Results = AddValueOutput(nameof(Results), GetResultsAsync);
        }

        protected abstract Task<object> GetResultsAsync(IFlow flow);

        protected HttpClient BuildClient(string apiKey)
        {
            var client = NodeClient.ClientFactory.CreateClient("AppInsight");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-api-key", apiKey);
            return client;
        }

        protected string BuildUrl(string appId, string query, string timespan = null)
        {
            var url = string.Format(AppInsightUrl, appId, HttpUtility.UrlEncode(query));
            if (!string.IsNullOrEmpty(timespan))
            {
                url += $"&timespan={timespan}";
            }

            return url;
        }
    }
}