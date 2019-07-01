using System;
using ValueType = Nodester.Graph.Core.Data.ValueType;

namespace Nodester.Data.Dto.ComponentDtos
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