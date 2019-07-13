using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Nodester.Services.Data;

namespace Nodester.Graph.Core.Nodes.HTTP
{
    public abstract class HttpNode : Node
    {
        protected readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private INodeHttpClient NodeClient { get; }

        protected HttpClient Client => NodeClient.Client;

        protected HttpNode(INodeHttpClient client)
        {
            NodeClient = client;
        }
    }
}