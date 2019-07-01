using System.Collections.Generic;
using Nodester.Data.Models.Json_Models;
using Nodester.Data.Models.Json_Models.Fields.FlowFields;
using Nodester.Data.Models.Json_Models.Fields.ValueFields;

namespace Nodester.Data.Models
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