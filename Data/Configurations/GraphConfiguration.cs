using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nodegem.Data.Extensions;

namespace Nodegem.Data.Configurations
{
    public class GraphConfiguration : IEntityTypeConfiguration<Models.Graph>
    {
        public void Configure(EntityTypeBuilder<Models.Graph> builder)
        {
            builder.Property(e => e.Nodes).StoreAsJson();
            builder.Property(e => e.Links).StoreAsJson();
            builder.Property(e => e.Constants).StoreAsJson();
            builder.Property(e => e.RecurringOptions).StoreAsJson();
            builder.Property(e => e.Metadata).StoreAsJson();
            builder.HasOne(e => e.User).WithMany(e => e.Graphs);
        }
    }
}