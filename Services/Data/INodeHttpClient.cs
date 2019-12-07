using System.Net.Http;

namespace Nodegem.Services.Data
{
    public interface INodeHttpClient
    {
        HttpClient Client { get; }
        IHttpClientFactory ClientFactory { get; }
    }
}