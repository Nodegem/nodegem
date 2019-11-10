using System;
using System.ComponentModel.DataAnnotations;

namespace Nodester.Data.Dto.MacroDtos
{
    /// <summary>
    ///  This DTO exists purely because macros can have links from their I/O which aren't explicitly nodes
    /// </summary>
    public class MacroLinkDto
    {
        public Guid? SourceNode { get; set; }

        [Required] public string SourceKey { get; set; }

        public Guid? DestinationNode { get; set; }

        [Required] public string DestinationKey { get; set; }
    }
}