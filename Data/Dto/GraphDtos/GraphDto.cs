using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nodester.Data.Dto.ComponentDtos;

namespace Nodester.Data.Dto.GraphDtos
{
    public class GraphDto
    {
        [Required] public Guid Id { get; set; }
        [Required] public Guid UserId { get; set; }

        public bool IsDebugModeEnabled { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<NodeDto> Nodes { get; set; }
        public IEnumerable<LinkDto> Links { get; set; }
        public IEnumerable<ConstantDto> Constants { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}