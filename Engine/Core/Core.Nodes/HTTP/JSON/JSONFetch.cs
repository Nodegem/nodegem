using System.Threading.Tasks;
using Newtonsoft.Json;
using Nodegem.Common.Data;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using Nodegem.Services.Data;

namespace Nodegem.Engine.Core.Nodes.HTTP.JSON
{
    [DefinedNode("27A8D1B7-7EFD-46CE-8900-151069E28056", Title = "JSON Fetch")]
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