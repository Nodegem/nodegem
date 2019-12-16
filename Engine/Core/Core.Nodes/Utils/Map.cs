using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Utils
{
    [DefinedNode("409A429A-9FFE-42B6-81F7-3198AFDB5DEA")]
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