using System.Net.Http;
using Nodegem.ClientService.Data;

namespace Nodegem.ClientService.Services
{
    public class NodesterApiService : INodesterApiService
    {
        public INodesterLoginService LoginService { get; }
        public INodesterGraphService GraphService { get; }
        public INodesterUserService UserService { get; }

        public NodesterApiService(HttpClient client)
        {
            LoginService = new NodesterLoginService(client);
            GraphService = new NodesterGraphService(client);
            UserService = new NodesterUserService(client);
        }
    }
}