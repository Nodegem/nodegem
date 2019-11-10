using System.ComponentModel.DataAnnotations;
using Nodester.Common.Data;

namespace Nodester.Common.Dto.ValueFieldDtos
{
    public abstract class ValueFieldDto : BaseFieldDto
    {
        [Required]
        public ValueType Type { get; set; }
    }
}