using System;
using System.ComponentModel.DataAnnotations;

namespace Nodegem.Common.Dto
{
    public abstract class BaseFieldDto
    {
        public Guid Key { get; set; } = Guid.NewGuid();

        [Required] public string Label { get; set; }
    }
}