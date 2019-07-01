using System;
using Nodester.Graph.Core.Data.Fields;
using Nodester.Graph.Core.Data.Links;
using Nodester.Graph.Core.Links.Graph;

namespace Nodester.Graph.Core.Fields.Graph
{
    public class ValueInput : ValueField, IValueInputField
    {
        public object DefaultValue { get; }
        public IValueLink Connection { get; private set; }

        public ValueInput(string key, object defaultValue, Type type) : base(key, type)
        {
            DefaultValue = defaultValue;
        }

        public override object GetValue()
        {
            return Value ?? DefaultValue;
        }

        public void SetConnection(IValueOutputField output)
        {
            Connection = new ValueLink(output, this);
        }
    }
}