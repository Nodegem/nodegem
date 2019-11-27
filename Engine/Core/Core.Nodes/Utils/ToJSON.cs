using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Utils
{
    [DefinedNode("02EC5BD2-CE4F-482B-9F7B-72BF53C9BF80", Title = "To JSON")]
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
                async flow =>
                {
                    var value = await flow.GetValueAsync<object>(Value);
                    return JToken.FromObject(value,
                        JsonSerializer.Create(SerializerSettings));
                });
        }
    }
}