using System.Net.Http;

namespace Nodester.Services.Data
{
    public interface INodeHttpClient
    {
        HttpClient Client { get; }
    }
}