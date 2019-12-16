using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Nodegem.Engine.Integrations.Data.Mailgun;
using Nodegem.Services.Data;

namespace Nodegem.Engine.Integrations.Mailgun
{
    public class MailgunService : IMailgunService
    {
        private const string MailgunBaseUri = "https://api.mailgun.net/v3/";

        private readonly HttpClient _client;

        public MailgunService(INodeHttpClient httpClient)
        {
            _client = httpClient.Client;
        }

        // Taken from https://elanderson.net/2017/02/email-with-asp-net-core-using-mailgun/
        public async Task SendEmailAsync(string apiKey, string requestUri, string to, string @from, string subject,
            string message)
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(Encoding.ASCII.GetBytes($"api:{apiKey}")));

            var content = new MultipartFormDataContent();

            var values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("from", from),
                new KeyValuePair<string, string>("to", to),
                new KeyValuePair<string, string>("subject", subject),
                new KeyValuePair<string, string>("text", message)
            };
            
            values.ForEach(v => content.Add(new StringContent(v.Value), v.Key));

            await _client.PostAsync($"{MailgunBaseUri}{requestUri}/messages",
                content);
        }
    }
}