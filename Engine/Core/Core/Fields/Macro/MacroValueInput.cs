using System;
using Nodester.Graph.Core.Data.Fields;
using Nodester.Graph.Core.Fields.Graph;

namespace Nodester.Graph.Core.Fields.Macro
{
    public class MacroValueInput : ValueOutput, IMacroValueInputField
    {
        public object DefaultValue { get; }

        public MacroValueInput(string key, object defaultValue, Type type) : base(key, type)
        {
            DefaultValue = defaultValue;
        }

        public override object GetValue()
        {
            return Value ?? DefaultValue;
        }
    }
}