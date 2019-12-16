using System.Net.Http;

namespace Nodegem.ClientService.Services
{
    public abstract class NodegemBaseService
    {
        protected HttpClient Client { get; }

        protected NodegemBaseService(HttpClient client)
        {
            Client = client;
        }
    }
}