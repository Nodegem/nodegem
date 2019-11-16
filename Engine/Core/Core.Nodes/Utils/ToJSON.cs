using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.Utils
{
    [DefinedNode("To JSON")]
    [NodeNamespace("Core.Utils")]
    public class ToJSON : Node
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.None
        };

        public IValueInputField Value { get; set; }
        public IValueOutputField JSON { get; set; }

        protected override void Define()
        {
            Value = AddValueInput<object>(nameof(Value));
            JSON = AddValueOutput(nameof(JSON),
                async flow => JObject.FromObject(await flow.GetValueAsync<object>(Value),
                    JsonSerializer.Create(SerializerSettings)));
        }
    }
}