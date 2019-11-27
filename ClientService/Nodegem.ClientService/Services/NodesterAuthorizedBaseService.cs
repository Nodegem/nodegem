using System.Net.Http;
using System.Net.Http.Headers;
using Nodegem.ClientService.Exceptions;

namespace Nodegem.ClientService.Services
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

                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AppState.Instance.Token.RawData}");

                return _client;
            }
        }

        protected NodesterAuthorizedBaseService(HttpClient client)
        {
            _client = client;
        }
    }
}