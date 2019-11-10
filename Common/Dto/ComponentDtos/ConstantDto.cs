using System;
using ValueType = Nodester.Common.Data.ValueType;

namespace Nodester.Common.Dto.ComponentDtos
{
    public class ConstantDto
    {
        public Guid Key { get; set; }
        public string Label { get; set; }
        public object Value { get; set; }
        public ValueType Type { get; set; }
        public bool IsSecret { get; set; }
    }
}