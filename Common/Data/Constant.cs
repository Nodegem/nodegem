using System;

namespace Nodegem.Common.Data
{
    public class Constant
    {
        public Guid Key { get; set; }
        public string Label { get; set; }
        public ValueType Type { get; set; }
        public object Value { get; set; }
    }
}