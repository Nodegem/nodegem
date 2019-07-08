using System.Net.Http;
using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.HTTP
{
    [DefinedNode]
    [NodeNamespace("Core.HTTP")]
    public class Fetch : Node
    {
        public IFlowInputField In { get; private set; }
        public IFlowOutputField Out { get; private set; }

        public IValueInputField Url { get; private set; }
        public IValueOutputField Data { get; private set; }

        protected override void Define()
        {
            In = AddFlowInput(nameof(In), FetchData);
            Out = AddFlowOutput(nameof(Out));

            Url = AddValueInput<string>(nameof(Url));
            Data = AddValueOutput(nameof(Data), RetrieveData);
        }

        private Task<IFlowOutputField> FetchData(IFlow flow)
        {
            return Task.FromResult(Out);
        }

        private object RetrieveData(IFlow flow)
        {
            using (var client = new HttpClient())
            {
                return client.GetAsync(flow.GetValue<string>(Url)).Result.Content.ReadAsStringAsync().Result;
            }
        }
    }
}