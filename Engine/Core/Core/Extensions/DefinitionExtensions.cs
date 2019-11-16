using Nodester.Engine.Data.Definitions;
using Nodester.Engine.Data.Fields;
using Nodester.Graph.Core.Fields.Graph;
using ValueType = Nodester.Common.Data.ValueType;

namespace Nodester.Graph.Core.Extensions
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