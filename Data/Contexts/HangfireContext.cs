using Microsoft.EntityFrameworkCore;

namespace Nodester.Data.Contexts
{
    public class HangfireContext : DbContext
    {
        
        public HangfireContext(DbContextOptions<HangfireContext> options) : base(options)
        {
        }
        
    }
}