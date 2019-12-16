using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nodegem.Data.Models;

namespace Nodegem.Data.Configurations
{
    public class AccessTokenConfiguration : IEntityTypeConfiguration<AccessToken>
    {
        public void Configure(EntityTypeBuilder<AccessToken> builder)
        {
            builder.HasAlternateKey(x => x.Token);
            builder.HasAlternateKey(x => x.UserId);
        }
    }
}