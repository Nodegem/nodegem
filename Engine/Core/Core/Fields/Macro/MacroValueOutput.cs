using System;
using Nodester.Engine.Data.Fields;
using Nodester.Engine.Data.Links;
using Nodester.Graph.Core.Links.Macro;

namespace Nodester.Graph.Core.Fields.Macro
{
    public class MacroValueOutput : ValueField, IMacroValueOutputField
    {
        public IMacroValueLink Connection { get; set; }

        public MacroValueOutput(string key, Type returnType) : base(key, returnType)
        {
        }

        public void SetConnection(IValueOutputField output)
        {
            Connection = new MacroValueLink(this, output);
        }
    }
}