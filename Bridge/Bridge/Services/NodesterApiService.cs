using System.Net.Http;
using Bridge.Data;

namespace Nodester.Bridge.Services
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