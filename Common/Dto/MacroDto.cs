using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nodegem.Common.Dto.ComponentDtos;
using Nodegem.Common.Dto.FlowFieldDtos;
using Nodegem.Common.Dto.ValueFieldDtos;

namespace Nodegem.Common.Dto
{
    public class MacroDto
    {
        [Required] 
        public Guid Id { get; set; }
        [Required] 
        public Guid UserId { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<NodeDto> Nodes { get; set; }
        public IEnumerable<MacroLinkDto> Links { get; set; }
        public IEnumerable<FlowInputDto> FlowInputs { get; set; }
        public IEnumerable<FlowOutputDto> FlowOutputs { get; set; }
        public IEnumerable<ValueInputDto> ValueInputs { get; set; }
        public IEnumerable<ValueOutputDto> ValueOutputs { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
    }
}