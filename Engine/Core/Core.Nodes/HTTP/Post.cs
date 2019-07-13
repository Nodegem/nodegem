using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using Nodester.Services.Data;

namespace Nodester.Graph.Core.Nodes.HTTP
{
    [DefinedNode]
    [NodeNamespace("Core.HTTP")]
    public class Post : HttpNode
    {
        
        public IValueInputField Url { get; set; }
        public IValueInputField Body { get; set; }
        public IValueOutputField Response { get; set; }
        
        public Post(INodeHttpClient client) : base(client)
        {
        }
        
        protected override void Define()
        {
            Url = AddValueInput<string>(nameof(Url));
            Body = AddValueInput<object>(nameof(Body));
            Response = AddValueOutput(nameof(Response), GetPostResponse);
        }

        private async Task<string> GetPostResponse(IFlow flow)
        {
            var url = await flow.GetValueAsync<string>(Url);
            var body = await flow.GetValueAsync<object>(Body);
            
            var httpContent = new StringContent(JsonConvert.SerializeObject(body, Settings));
            var response = await Client.PostAsync(new Uri(url), httpContent);

            return await response.Content.ReadAsStringAsync();
        }
    }
}