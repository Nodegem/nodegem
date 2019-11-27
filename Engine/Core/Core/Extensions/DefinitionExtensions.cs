using Nodegem.Engine.Data.Definitions;
using Nodegem.Engine.Data.Fields;
using ValueType = Nodegem.Common.Data.ValueType;

namespace Nodegem.Engine.Core.Extensions
{
    public static class DefinitionExtensions
    {
        public static FlowInputDefinition ToFlowInputDefinition(this IFlowInputField input, string label)
        {
            return new FlowInputDefinition
            {
                Label = label,
                Key = input.Key
            };
        }

        public static FlowOutputDefinition ToFlowOutputDefinition(this IFlowOutputField output, string label,
            bool isIndefinite)
        {
            return new FlowOutputDefinition
            {
                Label = label,
                Key = output.Key,
                Indefinite = isIndefinite
            };
        }

        public static ValueInputDefinition ToValueInputDefinition(this IValueInputField input, string label,
            ValueType type, bool isIndefinite, bool isEditable, bool allowConnection)
        {
            return new ValueInputDefinition
            {
                Label = label,
                DefaultValue = input.DefaultValue,
                Key = input.Key,
                ValueType = type,
                Indefinite = isIndefinite,
                IsEditable = isEditable,
                AllowConnection = allowConnection
            };
        }

        public static ValueOutputDefinition ToValueOutputDefinition(this IValueOutputField output, string label,
            ValueType type, bool isIndefinite)
        {
            return new ValueOutputDefinition
            {
                Label = label,
                Key = output.Key,
                ValueType = type,
                Indefinite = isIndefinite
            };
        }
    }
}