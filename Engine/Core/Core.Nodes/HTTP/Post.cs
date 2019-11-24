using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using Nodester.Services.Data;
using ValueType = Nodester.Common.Data.ValueType;

namespace Nodester.Graph.Core.Nodes.HTTP
{
    [DefinedNode]
    [NodeNamespace("Core.HTTP")]
    public class Post : HttpNode
    {
        
        [FieldAttributes(ValueType.Url)]
        public IValueInputField Url { get; set; }
        
        [FieldAttributes(IsEditable = false)]
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
            var response = await Client.PostAsync(new Uri(url), new StreamContent(Stream.Null));

            return await response.Content.ReadAsStringAsync();
        }
    }
}