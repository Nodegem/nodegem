using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using Nodester.Services.Data;

namespace Nodester.Graph.Core.Nodes.HTTP
{
    [DefinedNode("JSONFetch")]
    [NodeNamespace("Core.HTTP")]
    public class JSONFetch : HttpNode
    {
        public IValueInputField Url { get; private set; }
        public IValueOutputField Data { get; private set; }

        public JSONFetch(INodeHttpClient client) : base(client)
        {
        }

        protected override void Define()
        {
            Url = AddValueInput<string>(nameof(Url));
            Data = AddValueOutput(nameof(Data), RetrieveData);
        }

        private async Task<object> RetrieveData(IFlow flow)
        {
            var fetchResult = await Client.GetStringAsync(await flow.GetValueAsync<string>(Url));
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(fetchResult);
        }
    }
}