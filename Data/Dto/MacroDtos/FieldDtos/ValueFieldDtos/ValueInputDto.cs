using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nodester.Data.Dto.MacroDtos.FieldDtos.ValueFieldDtos
{
    public class ValueInputDto : ValueFieldDto
    {
        public object DefaultValue { get; set; }
    }
}