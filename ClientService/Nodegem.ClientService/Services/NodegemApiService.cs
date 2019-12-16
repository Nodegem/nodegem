using System.Net.Http;
using Nodegem.ClientService.Data;

namespace Nodegem.ClientService.Services
{
    public class NodegemApiService : INodegemApiService
    {
        public INodegemLoginService LoginService { get; }
        public INodegemGraphService GraphService { get; }
        public INodegemUserService UserService { get; }

        public NodegemApiService(HttpClient client)
        {
            LoginService = new NodegemLoginService(client);
            GraphService = new NodegemGraphService(client);
            UserService = new NodegemUserService(client);
        }
    }
}