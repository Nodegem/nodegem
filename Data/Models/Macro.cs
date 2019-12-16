using System.Collections.Generic;
using Nodegem.Data.Models.Json_Models;
using Nodegem.Data.Models.Json_Models.Fields.FlowFields;
using Nodegem.Data.Models.Json_Models.Fields.ValueFields;

namespace Nodegem.Data.Models
{
    public class Macro : BaseGraph
    {
        public IEnumerable<FlowInput> FlowInputs { get; set; }
        public IEnumerable<FlowOutput> FlowOutputs { get; set; }
        public IEnumerable<ValueInput> ValueInputs { get; set; }
        public IEnumerable<ValueOutput> ValueOutputs { get; set; }
        public IEnumerable<MacroLink> Links { get; set; }
    }
}