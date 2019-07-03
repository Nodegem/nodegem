using System;
using System.Net.Http;
using Microsoft.Extensions.Options;
using Nodester.Bridge.Exceptions;

namespace Nodester.Bridge.Services
{
    public abstract class NodesterAuthorizedBaseService
    {
        private readonly HttpClient _client;

        protected HttpClient Client
        {
            get
            {
                if (!AppState.Instance.IsLoggedIn)
                {
                    throw new NodesterAuthorizationException();
                }

                if (!_client.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _client.DefaultRequestHeaders.Add("Authorization",
                        $"Bearer {AppState.Instance.Token.RawData}");
                }

                return _client;
            }
        }

        protected NodesterAuthorizedBaseService(HttpClient client, IOptions<AppConfig> config)
        {
            var host = config.Value.Host;

            client.BaseAddress = new Uri($"{host}/api/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            _client = client;
        }
    }
}