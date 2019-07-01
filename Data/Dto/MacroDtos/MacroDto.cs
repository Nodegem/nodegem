using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nodester.Data.Dto.ComponentDtos;
using Nodester.Data.Dto.MacroDtos.FieldDtos.FlowFieldDtos;
using Nodester.Data.Dto.MacroDtos.FieldDtos.ValueFieldDtos;

namespace Nodester.Data.Dto.MacroDtos
{
    public class MacroDto
    {
        [Required] 
        public Guid Id { get; set; }
        [Required] 
        public Guid UserId { get; set; }
        
        public bool IsDebugModeEnabled { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<NodeDto> Nodes { get; set; }
        public IEnumerable<MacroLinkDto> Links { get; set; }
        public IEnumerable<FlowInputDto> FlowInputs { get; set; }
        public IEnumerable<FlowOutputDto> FlowOutputs { get; set; }
        public IEnumerable<ValueInputDto> ValueInputs { get; set; }
        public IEnumerable<ValueOutputDto> ValueOutputs { get; set; }
    }
}