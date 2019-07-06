using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nodester.Data.Dto.ComponentDtos;

namespace Nodester.Data.Dto.GraphDtos
{
    public class CreateGraphDto
    {
        [Required] public string Name { get; set; }

        [Required] public string Description { get; set; }

        [Required] public Guid UserId { get; set; }
        
        public RecurringOptionsDto RecurringOptions { get; set; }
        
        public IEnumerable<ConstantDto> Constants { get; set; }
    }
}