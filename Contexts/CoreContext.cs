using Common.Data;
using DatabaseService.Data.Models;
using DatabaseService.Data.KafkaEvents;
using Microsoft.EntityFrameworkCore;

namespace DatabaseService.Contexts
{
    public class CoreContext : DbContext
    {
        public CoreContext(DbContextOptions<CoreContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<ApiScope> ApiScopes { get; set; }
        public DbSet<ApiResource> ApiResources { get; set; }
        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Name)
                .IsUnique();

            modelBuilder.Entity<ApiResource>()
                .HasMany(r => r.Scopes)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.AllowedScopes)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
