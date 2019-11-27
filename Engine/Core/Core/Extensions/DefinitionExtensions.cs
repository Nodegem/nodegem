using System.Collections.Generic;
using Nodegem.Engine.Data.Definitions;
using Nodegem.Engine.Data.Fields;
using ValueType = Nodegem.Common.Data.ValueType;

namespace Nodegem.Engine.Core.Extensions
{
    public static class DefinitionExtensions
    {
        public static FlowInputDefinition ToFlowInputDefinition(this IFlowInputField input, string label, string info)
        {
            return new FlowInputDefinition
            {
                Label = label,
                Key = input.Key,
                Info = info
            };
        }

        public static FlowOutputDefinition ToFlowOutputDefinition(this IFlowOutputField output, string label,
            bool isIndefinite, string info)
        {
            return new FlowOutputDefinition
            {
                Label = label,
                Key = output.Key,
                Indefinite = isIndefinite,
                Info = info
            };
        }

        public static ValueInputDefinition ToValueInputDefinition(this IValueInputField input, string label,
            ValueType type, bool isIndefinite, bool isEditable, bool allowConnection, string info,
            IEnumerable<object>? valueOptions)
        {
            return new ValueInputDefinition
            {
                Label = label,
                DefaultValue = input.DefaultValue,
                Key = input.Key,
                ValueType = type,
                Indefinite = isIndefinite,
                IsEditable = isEditable,
                AllowConnection = allowConnection,
                Info = info,
                ValueOptions = valueOptions
            };
        }

        public static ValueOutputDefinition ToValueOutputDefinition(this IValueOutputField output, string label,
            ValueType type, bool isIndefinite, string info)
        {
            return new ValueOutputDefinition
            {
                Label = label,
                Key = output.Key,
                ValueType = type,
                Indefinite = isIndefinite,
                Info = info
            };
        }
    }
}