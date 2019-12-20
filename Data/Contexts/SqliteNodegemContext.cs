using Microsoft.EntityFrameworkCore;

namespace Nodegem.Data.Contexts
{
    //Only used for migrations
    public class SqliteNodegemContext : NodegemContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=NodegemDatabase.db", b => b.MigrationsAssembly("Nodegem.WebApi"));
    }
}