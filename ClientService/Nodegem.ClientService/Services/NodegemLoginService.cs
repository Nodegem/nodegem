using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Nodegem.ClientService.Data;
using Nodegem.ClientService.Extensions;
using Nodegem.Data.Dto;

namespace Nodegem.ClientService.Services
{
    public class NodegemLoginService : NodegemBaseService, INodegemLoginService
    {
        public NodegemLoginService(HttpClient client) : base(client)
        {
        }

        public async Task<TokenDto> GetAccessTokenAsync(string username, string password)
        {
            var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
            Client.DefaultRequestHeaders.Add("Authorization", $"Basic {encoded}");
            return await Client.GetAsync<TokenDto>("account/token");
        }
    }
}