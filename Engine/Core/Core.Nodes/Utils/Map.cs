using System.Collections.Generic;
using System.Threading.Tasks;
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
            if (!(@object is IDictionary<string, object> dictionary))
            {
                dictionary = @object.ToDictionary();
            }

            var outputField = GetFieldByKey(key) as IValueOutputField;
            var objectKey = outputField.GetValue() as string;
            if (dictionary.ContainsKey(objectKey))
            {
                return dictionary[objectKey];
            }

            throw new KeyNotFoundException("Key does not exist");
        }
    }
}