using System;
using System.ComponentModel.DataAnnotations;

namespace Nodegem.Data.Models.Json_Models
{
    public class MacroLink
    {
        public Guid? SourceNode { get; set; }

        [Required] public string SourceKey { get; set; }

        public Guid? DestinationNode { get; set; }

        [Required] public string DestinationKey { get; set; }
    }
}