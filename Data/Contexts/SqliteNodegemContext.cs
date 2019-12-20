using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nodegem.Common.Data;
using Nodegem.Data.Models;

namespace Nodegem.Data.Contexts
{
    //Only used for migrations
    public class SqliteNodegemContext : NodegemContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=NodegemDatabase.db", b => b.MigrationsAssembly("Nodegem.WebApi"));

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                EmailConfirmed = true,
                Email = "nodegemdefault@nodegem.io",
                NormalizedEmail = "NODEGEMDEFAULT@NODEGEM.IO",
                UserName = "Nodegem_Default",
                NormalizedUserName = "NODEGEM_DEFAULT",
                SecurityStamp = Guid.NewGuid().ToString("D"),
                FirstName = "Nodegem",
                LastName = "Default",
                Graphs = new List<Graph>(),
                Macros = new List<Macro>(),
                Constants = new List<Constant>(),
                CreatedOn = DateTime.UtcNow,
                IsActive = true,
            };
            
            user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, "P@ssword1");
            builder.Entity<ApplicationUser>().HasData(user);
        }
    }
}