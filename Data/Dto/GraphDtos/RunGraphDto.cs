using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nodester.Data.Dto.ComponentDtos;

namespace Nodester.Data.Dto.GraphDtos
{
    public class RunGraphDto
    {
        [Required] public Guid Id { get; set; }

        [Required] public IEnumerable<NodeDto> Nodes { get; set; }

        [Required] public IEnumerable<LinkDto> Links { get; set; }
        
        public bool IsDebugModeEnabled { get; set; }
        
    }
}