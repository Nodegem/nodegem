using System.ComponentModel.DataAnnotations;
using Nodegem.Common.Data;

namespace Nodegem.Common.Dto.ValueFieldDtos
{
    public abstract class ValueFieldDto : BaseFieldDto
    {
        [Required]
        public ValueType Type { get; set; }
    }
}