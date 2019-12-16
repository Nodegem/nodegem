using System.Collections.Generic;

namespace Nodegem.Engine.Data.Definitions
{
    public class ValueInputDefinition : ValueFieldDefinition
    {
        public object DefaultValue { get; set; }
        public bool IsEditable { get; set; }
        public bool AllowConnection { get; set; }
        public IEnumerable<ValueOption> ValueOptions { get; set; }
    }
}