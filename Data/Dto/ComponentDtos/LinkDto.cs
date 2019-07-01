using System;
using System.ComponentModel.DataAnnotations;

namespace Nodester.Data.Dto.ComponentDtos
{
    public class LinkDto
    {
        [Required] public Guid SourceNode { get; set; }

        [Required] public string SourceKey { get; set; }

        [Required] public Guid DestinationNode { get; set; }

        [Required] public string DestinationKey { get; set; }
    }
}