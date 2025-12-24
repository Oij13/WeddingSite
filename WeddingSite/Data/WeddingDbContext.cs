using Microsoft.EntityFrameworkCore;
using WeddingSite.Data.Models;

namespace WeddingSite.Data
{
    public class WeddingDbContext : DbContext
    {
        public WeddingDbContext(DbContextOptions<WeddingDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Guest> Guests { get; set; } = default!;
        public DbSet<RegistryItem> RegistryItems { get; set; } = default!;
        public DbSet<Photo> Photos { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // simple configuration examples
            modelBuilder.Entity<Guest>()
                .Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<RegistryItem>()
                .HasOne(ri => ri.Photo)
                .WithMany()
                .HasForeignKey(ri => ri.PhotoId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Photo>()
                .Property(p => p.Path)
                .IsRequired()
                .HasMaxLength(500);
        }
    }
}
