using System;
using Nodegem.Engine.Core.Links.Macro;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Data.Links;
using Nodegem.Engine.Data.Nodes;

namespace Nodegem.Engine.Core.Fields.Macro
{
    public class MacroValueOutput : ValueField, IMacroValueOutputField
    {
        public IMacroValueLink Connection { get; set; }

        public MacroValueOutput(string key, Type returnType) : base(key, returnType)
        {
        }
        
        public MacroValueOutput(string key, Type returnType, INode node) : base(key, returnType, node)
        {
        }

        public void SetConnection(IValueOutputField output)
        {
            Connection = new MacroValueLink(this, output);
        }
    }
}