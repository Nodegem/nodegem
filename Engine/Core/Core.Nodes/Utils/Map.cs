using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Nodester.Common.Extensions;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.Utils
{
    [DefinedNode]
    [NodeNamespace("Core.Utils")]
    public class Map : Node
    {
        
        [FieldAttributes(IsEditable = false)]
        public IValueInputField Object { get; set; }
        
        [FieldAttributes(nameof(Keys))]
        public IEnumerable<IValueOutputField> Keys { get; set; }
        
        protected override void Define()
        {
            Object = AddValueInput<object>(nameof(Object));
            Keys = InitializeValueOutputList<object>(nameof(Keys), GetValueFromObjectAsync);
        }

        private async Task<object> GetValueFromObjectAsync(IFlow flow, string key)
        {
            var @object = await flow.GetValueAsync<object>(Object);
            JObject jObject;
            if (!(@object is JObject))
            {
                jObject = JObject.FromObject(@object);
            }
            else
            {
                jObject = @object as JObject;
            }

            var outputField = GetFieldByKey(key) as IValueOutputField;
            var objectKey = outputField.GetValue() as string;
            if (jObject.ContainsKey(objectKey))
            {
                return jObject[objectKey];
            }

            throw new KeyNotFoundException("Key does not exist");
        }
    }
}