using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Nodester.Engine.Data;

namespace Nodester.Data.Dto.MacroDtos.FieldDtos.ValueFieldDtos
{
    public abstract class ValueFieldDto : BaseFieldDto
    {
        [Required]
        public ValueType Type { get; set; }
    }
}