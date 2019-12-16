using System.Threading.Tasks;
using Nodegem.Common.Data;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using Nodegem.Services.Data;

namespace Nodegem.Engine.Core.Nodes.HTTP
{
    [DefinedNode("96D8911B-A102-4955-8A72-362217D1C365")]
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