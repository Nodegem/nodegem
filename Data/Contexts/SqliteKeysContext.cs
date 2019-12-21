using Microsoft.EntityFrameworkCore;

namespace Nodegem.Data.Contexts
{
    //Only used for migrations
    public class SqliteKeysContext : KeysContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=KeysDatabase.db", b => b.MigrationsAssembly("Nodegem.WebApi"));
    }
}