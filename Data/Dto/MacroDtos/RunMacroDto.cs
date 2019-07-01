using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nodester.Data.Dto.ComponentDtos;
using Nodester.Data.Dto.MacroDtos.FieldDtos.FlowFieldDtos;
using Nodester.Data.Dto.MacroDtos.FieldDtos.ValueFieldDtos;

namespace Nodester.Data.Dto.MacroDtos
{
    public class RunMacroDto
    {
        [Required] public Guid Id { get; set; }

        [Required] public IEnumerable<NodeDto> Nodes { get; set; }

        [Required] public IEnumerable<MacroLinkDto> Links { get; set; }
        
        public bool IsDebugModeEnabled { get; set; }

        public IEnumerable<FlowInputDto> FlowInputs { get; set; } = new List<FlowInputDto>();

        public IEnumerable<FlowOutputDto> FlowOutputs { get; set; } = new List<FlowOutputDto>();

        public IEnumerable<ValueInputDto> ValueInputs { get; set; } = new List<ValueInputDto>();

        public IEnumerable<ValueOutputDto> ValueOutputs { get; set; } = new List<ValueOutputDto>();
    }
}