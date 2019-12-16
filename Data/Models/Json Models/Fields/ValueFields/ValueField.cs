using Nodegem.Common.Data;

namespace Nodegem.Data.Models.Json_Models.Fields.ValueFields
{
    public abstract class ValueField : BaseField
    {
        public ValueType Type { get; set; }
        public object DefaultValue { get; set; }
    }
}