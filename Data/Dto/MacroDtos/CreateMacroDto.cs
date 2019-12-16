using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nodegem.Common.Dto.FlowFieldDtos;
using Nodegem.Common.Dto.ValueFieldDtos;

namespace Nodegem.Data.Dto.MacroDtos
{
    public class CreateMacroDto
    {
        [Required] 
        public string Name { get; set; }

        public string Description { get; set; }
        
        [Required]
        public Guid UserId { get; set; }
        
        public IEnumerable<FlowInputDto> FlowInputs { get; set; }
        
        public IEnumerable<FlowOutputDto> FlowOutputs { get; set; }
        
        public IEnumerable<ValueInputDto> ValueInputs { get; set; }
        
        public IEnumerable<ValueOutputDto> ValueOutputs { get; set; }
        
    }
}