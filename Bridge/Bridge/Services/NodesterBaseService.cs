using System;
using System.Net.Http;
using Microsoft.Extensions.Options;

namespace Nodester.Bridge.Services
{
    public abstract class NodesterBaseService
    {
        protected HttpClient Client { get; }

        protected NodesterBaseService(HttpClient client, IOptions<AppConfig> config)
        {
            var host = config.Value.Host;

            client.BaseAddress = new Uri($"{host}/api/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            Client = client;
        }
    }
}