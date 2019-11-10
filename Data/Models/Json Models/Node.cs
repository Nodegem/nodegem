using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nodester.Common.Data;
using Nodester.Engine.Data;

namespace Nodester.Data.Models.Json_Models
{
    public class Node
    {
        [Required] public Guid Id { get; set; }

        public Guid? MacroId { get; set; }
        public Guid? MacroFieldId { get; set; }
        public string FullName { get; set; }
        public Vector2 Position { get; set; }
        public List<FieldData> FieldData { get; set; }
        public bool Permanent { get; set; }
    }
}