using System.Net.Http;
using Nodegem.Services.Data;

namespace Nodegem.Services
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