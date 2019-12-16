using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nodegem.Data.Extensions;
using Nodegem.Data.Models;

namespace Nodegem.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.Constants).StoreAsJson();

            builder.HasMany(x => x.Macros).WithOne(x => x.User);
            builder.HasMany(x => x.Graphs).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);

            builder.HasData(SeedUsers());
        }

        private IEnumerable<ApplicationUser> SeedUsers()
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = "AdminUser",
                NormalizedUserName = "ADMINUSER",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                Email = "admin@admin.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("N")
            };

            var passwordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, "adminP@ssword1");
            user.PasswordHash = passwordHash;

            return new[]
            {
                user
            };
        }
    }
}