using System.Threading.Tasks;
using Newtonsoft.Json;
using Nodester.Common.Data;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using Nodester.Services.Data;

namespace Nodester.Graph.Core.Nodes.HTTP
{
    [DefinedNode("JSON Fetch")]
    [NodeNamespace("Core.HTTP.JSON")]
    public class JSONFetch : HttpNode
    {
        [FieldAttributes(ValueType.Url)]
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
            return JsonConvert.DeserializeObject(fetchResult);
        }
    }
}