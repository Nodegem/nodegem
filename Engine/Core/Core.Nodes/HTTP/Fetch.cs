using System.Net.Http;
using Nodester.Graph.Core.Data;
using Nodester.Graph.Core.Data.Attributes;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Nodes.HTTP
{
    [DefinedNode]
    [NodeNamespace("Core.HTTP")]
    public class Fetch : Node
    {
        public FlowInput In { get; private set; }
        public FlowOutput Out { get; private set; }

        public ValueInput Url { get; private set; }
        public ValueOutput Data { get; private set; }

        protected override void Define()
        {
            In = AddFlowInput(nameof(In), FetchData);
            Out = AddFlowOutput(nameof(Out));

            Url = AddValueInput<string>(nameof(Url));
            Data = AddValueOutput<object>(nameof(Data), RetrieveData);
        }

        private FlowOutput FetchData(IFlow flow)
        {
            CachedValue.SetValue(RetrieveData(flow));
            return Out;
        }

        private object RetrieveData(IFlow flow)
        {
            // TODO: Should probably use httpclientfactory
            using (var client = new HttpClient())
            {
                return client.GetAsync(flow.GetValue<string>(Url)).Result.Content.ReadAsStringAsync().Result;
            }
        }
    }
}