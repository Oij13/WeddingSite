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
        // Run this in Package Manager Console after changing a model/schema only if using VSCode, not VS

        // dotnet ef migrations add SyncModel --project WeddingSite --startup-project WeddingSite -o Data/Migrations
        // dotnet ef database update --project WeddingSite --startup-project WeddingSite
        public DbSet<Guest> Guests { get; set; } = default!;
        public DbSet<Photo> Photos { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("public");

            modelBuilder.Entity<Guest>(entity =>
            {
                // Invitation code must be unique
                entity.HasIndex(e => e.InvitationCode).IsUnique();

                // Email index for faster lookups
                entity.HasIndex(e => e.Email);
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.Property(p => p.Url)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(p => p.PublicId)
                    .IsRequired()
                    .HasMaxLength(200);
            });
        }
    }
}
