using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Services.Data;

namespace Nodegem.Engine.Core.Nodes.HTTP
{
    [NodeNamespace("Core.HTTP")]
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