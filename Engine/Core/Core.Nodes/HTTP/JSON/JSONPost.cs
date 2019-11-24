using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using Nodester.Services.Data;
using ValueType = Nodester.Common.Data.ValueType;

namespace Nodester.Graph.Core.Nodes.HTTP
{
    [DefinedNode("JSON Post")]
    [NodeNamespace("Core.HTTP.JSON")]
    public class JSONPost : HttpNode
    {
        
        [FieldAttributes(ValueType.Url)]
        public IValueInputField Url { get; set; }
        
        [FieldAttributes("JSON Body", ValueType.TextArea)]
        public IValueInputField JsonBody { get; set; }
        
        public IValueOutputField Response { get; set; }
        
        public JSONPost(INodeHttpClient client) : base(client)
        {
        }

        protected override void Define()
        {
            Url = AddValueInput(nameof(Url), default(string));
            JsonBody = AddValueInput(nameof(JsonBody), default(string));
            Response = AddValueOutput(nameof(Response), MakePostAsync);
        }

        private async Task<object> MakePostAsync(IFlow flow)
        {
            var url = await flow.GetValueAsync<string>(Url);
            var jsonBody = await flow.GetValueAsync<object>(JsonBody);
            var jsonString = !(jsonBody is string) ? JsonConvert.SerializeObject(jsonBody) : jsonBody as string;
            var result = await Client.PostAsync(new Uri(url), new StringContent(jsonString, Encoding.UTF8));
            return JsonConvert.DeserializeObject(await result.Content.ReadAsStringAsync());
        }
    }
}