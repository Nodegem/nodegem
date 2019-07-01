using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nodester.Data.Configurations;
using Nodester.Data.Models;
using Nodester.Data.Models.Json_Models;

namespace Nodester.Data.Contexts
{
    public class NodesterDBContext : IdentityDbContext<ApplicationUser, Role, Guid>
    {
        public DbSet<AccessToken> AccessTokens { get; set; }
        public DbSet<Models.Graph> Graphs { get; set; }
        public DbSet<Macro> Macros { get; set; }

        public NodesterDBContext(DbContextOptions<NodesterDBContext> options) : base(options)
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