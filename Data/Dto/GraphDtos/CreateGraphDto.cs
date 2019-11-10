using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nodester.Common.Data;
using Nodester.Common.Dto;
using Nodester.Common.Dto.ComponentDtos;

namespace Nodester.Data.Dto.GraphDtos
{
    public class CreateGraphDto
    {
        [Required] public string Name { get; set; }

        public string Description { get; set; }

        [Required] public Guid UserId { get; set; }
        
        public ExecutionType Type { get; set; }
        
        public RecurringOptionsDto RecurringOptions { get; set; }
        
        public IEnumerable<ConstantDto> Constants { get; set; }
    }
}