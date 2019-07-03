using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nodester.Data.Models;
using Nodester.Data.Models.Json_Models;
using Nodester.Engine.Data;

namespace Nodester.Data.Dto.ComponentDtos
{
    public class NodeDto
    {
        [Required] public Guid Id { get; set; }

        public Guid? MacroId { get; set; }
        public Guid? MacroFieldId { get; set; }
        public string FullName { get; set; }
        public Vector2 Position { get; set; }
        public IEnumerable<FieldData> FieldData { get; set; } = new List<FieldData>();
    }
}