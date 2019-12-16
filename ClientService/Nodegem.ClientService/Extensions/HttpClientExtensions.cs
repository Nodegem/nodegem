using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Nodegem.ClientService.Extensions
{
    public static class HttpClientExtensions
    {

        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };
        
        public static async Task<T> GetAsync<T>(this HttpClient client, string path)
        {
            var result = await client.GetAsync(path);
            return JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync(), Settings);
        }
    }
}