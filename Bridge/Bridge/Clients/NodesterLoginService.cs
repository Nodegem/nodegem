using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bridge.Data;
using Bridge.Data.Dtos;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Nodester.Bridge.Extensions;

namespace Nodester.Bridge.Clients
{
    public class NodesterLoginService : INodesterLoginService
    {

        private readonly HttpClient _client;

        public NodesterLoginService(HttpClient client, IOptions<AppConfig> config)
        {
            var host = config.Value.Host;

            client.BaseAddress = new Uri($"{host}/api/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            _client = client;
        }
        
        public async Task<TokenDto> GetAccessTokenAsync(string username, string password)
        {
            try
            {
                var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
                _client.DefaultRequestHeaders.Add("Authorization", $"Basic {encoded}");
                return await _client.GetAsync<TokenDto>("account/token");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception();
            }
        }
    }
}