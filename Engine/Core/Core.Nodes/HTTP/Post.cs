using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using Nodegem.Services.Data;
using ValueType = Nodegem.Common.Data.ValueType;

namespace Nodegem.Engine.Core.Nodes.HTTP
{
    [DefinedNode("06181B68-443E-4837-BF14-8033D8F6E4A8")]
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