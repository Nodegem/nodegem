using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    public struct Vector2
    {
        public static Vector2 Default = new Vector2 {X = 0, Y = 0};
        public float X { get; set; }
        public float Y { get; set; }
    }
}