using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nodegem.Common.Data;

namespace Nodegem.Data.Models.Json_Models
{
    public class Node
    {
        [Required] public Guid Id { get; set; }

        public string DefinitionId { get; set; }
        public Guid? MacroId { get; set; }
        public Guid? MacroFieldId { get; set; }
        public Guid? ConstantId { get; set; }
        public Vector2 Position { get; set; }
        public List<FieldData> FieldData { get; set; }
        public bool Permanent { get; set; }
    }
}