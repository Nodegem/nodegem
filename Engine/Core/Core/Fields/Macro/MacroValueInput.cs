using System;
using Nodegem.Engine.Core.Fields.Graph;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Fields.Macro
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