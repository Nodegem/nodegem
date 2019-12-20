using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Nodegem.Data.Contexts
{
    public class KeysContext : DbContext, IDataProtectionKeyContext
    {

        public KeysContext()
        {
        }
        
        public KeysContext(DbContextOptions<KeysContext> options)
            : base(options)
        {
        }

        // This maps to the table that stores keys.
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
    }
}