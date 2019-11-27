using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nodegem.Data.Extensions;
using Nodegem.Data.Models;

namespace Nodegem.Data.Configurations
{
    public class MacroConfiguration : IEntityTypeConfiguration<Macro>
    {
        public void Configure(EntityTypeBuilder<Macro> builder)
        {
            builder.Property(x => x.Nodes).StoreAsJson();
            builder.Property(x => x.Links).StoreAsJson();
            builder.Property(x => x.FlowInputs).StoreAsJson();
            builder.Property(x => x.FlowOutputs).StoreAsJson();
            builder.Property(x => x.ValueInputs).StoreAsJson();
            builder.Property(x => x.ValueOutputs).StoreAsJson();
            builder.HasOne(e => e.User).WithMany(e => e.Macros);
        }
    }
}