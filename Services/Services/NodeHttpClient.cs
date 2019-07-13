using System.Net.Http;
using Nodester.Services.Data;

namespace Nodester.Services
{
    public class NodeHttpClient : INodeHttpClient
    {
        public HttpClient Client { get; }
        
        public NodeHttpClient(HttpClient client)
        {
            Client = client;
        }
    }
}