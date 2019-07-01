using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nodester.Data.Models
{
    public abstract class BaseEntity : IEntity
    {
        [Key] public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}