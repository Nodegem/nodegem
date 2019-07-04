using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Nodester.Data.Models.Json_Models.Graph_Constants;

namespace Nodester.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>, IEntity, IActiveEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }

        [Required] [EmailAddress] public override string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public IEnumerable<Constant> Constants { get; set; } = new List<Constant>();

        [NotMapped] public string FullName => $"{FirstName} {LastName}";

        public DateTime LastLoggedIn { get; set; }

        public bool IsLocked { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedOn { get; set; }

        public DateTime LastUpdated { get; set; }

        public IEnumerable<Graph> Graphs { get; set; }

        public IEnumerable<Macro> Macros { get; set; }
    }
}