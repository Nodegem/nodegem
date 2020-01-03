using System;
using Nodegem.Engine.Core.Links.Graph;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Data.Links;
using Nodegem.Engine.Data.Nodes;

namespace Nodegem.Engine.Core.Fields.Graph
{
    public class ValueInput : ValueField, IValueInputField
    {
        public object DefaultValue { get; }
        public IValueLink Connection { get; private set; }

        public ValueInput(string key, object defaultValue, Type type, INode node) : base(key, type, node)
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