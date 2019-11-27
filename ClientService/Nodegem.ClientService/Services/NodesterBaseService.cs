using System.Net.Http;

namespace Nodegem.ClientService.Services
{
    public abstract class NodesterBaseService
    {
        protected HttpClient Client { get; }

        protected NodesterBaseService(HttpClient client)
        {
            Client = client;
        }
    }
}