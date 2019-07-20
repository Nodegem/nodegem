using Nodester.Engine.Data.Definitions;
using Nodester.Engine.Data.Fields;
using Nodester.Graph.Core.Fields.Graph;
using ValueType = Nodester.Engine.Data.ValueType;

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

        public static FlowOutputDefinition ToFlowOutputDefinition(this IFlowOutputField output, string label)
        {
            return new FlowOutputDefinition
            {
                Label = label,
                Key = output.Key
            };
        }

        public static ValueInputDefinition ToValueInputDefinition(this IValueInputField input, string label, ValueType type, bool isIndefinite)
        {
            return new ValueInputDefinition
            {
                Label = label,
                DefaultValue = input.DefaultValue,
                Key = input.Key,
                ValueType = type,
                Indefinite = isIndefinite
            };
        }

        public static ValueOutputDefinition ToValueOutputDefinition(this IValueOutputField output, string label)
        {
            return new ValueOutputDefinition
            {
                Label = label,
                Key = output.Key
            };
        }
    }
}