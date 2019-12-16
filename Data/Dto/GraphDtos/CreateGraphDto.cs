using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nodegem.Common.Data;
using Nodegem.Common.Dto;
using Nodegem.Common.Dto.ComponentDtos;

namespace Nodegem.Data.Dto.GraphDtos
{
    public class CreateGraphDto
    {
        [Required] public string Name { get; set; }

        public string Description { get; set; }

        [Required] public Guid UserId { get; set; }
        
        public ExecutionType Type { get; set; }
        
        public RecurringOptionsDto RecurringOptions { get; set; }
        
        public IEnumerable<Constant> Constants { get; set; }
    }
}