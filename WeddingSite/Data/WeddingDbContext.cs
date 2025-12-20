using Microsoft.EntityFrameworkCore;

namespace WeddingSite.Data
{
    public class WeddingDbContext : DbContext
    {
        public WeddingDbContext(DbContextOptions<WeddingDbContext> options) 
            : base(options)
        {
        }

        // Add your DbSet properties here
        // Example: public DbSet<Guest> Guests { get; set; }
    }
}
