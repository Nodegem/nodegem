using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nodegem.Data.Configurations;
using Nodegem.Data.Models;

namespace Nodegem.Data.Contexts
{
    public class NodegemContext : IdentityDbContext<ApplicationUser, Role, Guid>
    {
        public DbSet<AccessToken> AccessTokens { get; set; }
        public DbSet<Models.Graph> Graphs { get; set; }
        public DbSet<Macro> Macros { get; set; }

        public NodegemContext(DbContextOptions<NodegemContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new GraphConfiguration());
            builder.ApplyConfiguration(new MacroConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new AccessTokenConfiguration());
        }
    }
}