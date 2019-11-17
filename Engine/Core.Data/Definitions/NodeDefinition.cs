using System;
using System.Collections.Generic;

namespace Nodester.Engine.Data.Definitions
{
    public class NodeDefinition
    {
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IgnoreDisplay { get; set; }
        public bool IsListenerOnly { get; set; }
        public List<FlowInputDefinition> FlowInputs { get; set; }
        public List<FlowOutputDefinition> FlowOutputs { get; set; }
        public List<ValueInputDefinition> ValueInputs { get; set; }
        public List<ValueOutputDefinition> ValueOutputs { get; set; }
        public Guid? MacroId { get; set; }
        public Guid? MacroFieldId { get; set; }
    }
}