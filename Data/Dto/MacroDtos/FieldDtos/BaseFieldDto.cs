using System;
using System.ComponentModel.DataAnnotations;

namespace Nodester.Data.Dto.MacroDtos.FieldDtos
{
    public abstract class BaseFieldDto
    {
        public Guid Key { get; set; } = Guid.NewGuid();

        [Required] public string Label { get; set; }
    }
}