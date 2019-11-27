using System;
using ValueType = Nodegem.Common.Data.ValueType;

namespace Nodegem.Data.Models.Json_Models.Graph_Constants
{
    public class Constant
    {
        public Guid Key { get; set; }
        public string Label { get; set; }
        public object Value { get; set; }
        public ValueType Type { get; set; }
        public bool IsSecret { get; set; }
    }
}