using System.Threading.Tasks;
using Nodester.Common.Data;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using Nodester.Services.Data;

namespace Nodester.Graph.Core.Nodes.HTTP
{
    [DefinedNode]
    [NodeNamespace("Core.HTTP")]
    public class Fetch : HttpNode
    {
        [FieldAttributes(ValueType.Url)]
        public IValueInputField Url { get; private set; }
        public IValueOutputField Data { get; private set; }

        public Fetch(INodeHttpClient client) : base(client)
        {
        }

        protected override void Define()
        {
            Url = AddValueInput<string>(nameof(Url));
            Data = AddValueOutput(nameof(Data), RetrieveData);
        }

        private async Task<object> RetrieveData(IFlow flow)
        {
            var url = await flow.GetValueAsync<string>(Url);
            return await Client.GetAsync(url);
        }
    }
}