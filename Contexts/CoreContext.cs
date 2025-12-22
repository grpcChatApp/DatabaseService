using DatabaseService.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseService.Contexts
{
    public class CoreContext : DbContext
    {
        public CoreContext(DbContextOptions<CoreContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ApiScope> ApiScopes { get; set; }
        public DbSet<ApiResource> ApiResources { get; set; }
        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Id)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Id)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "user_role_mappings",
                    j => j.HasOne<Role>().WithMany().HasForeignKey("role_id"),
                    j => j.HasOne<User>().WithMany().HasForeignKey("user_id"),
                    j => j.ToTable("user_role_mappings", "auth") 
                );
            
            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Id)
                .IsUnique();

            modelBuilder.Entity<ApiScope>()
                .HasIndex(s => s.Id)
                .IsUnique();

            modelBuilder.Entity<ApiResource>()
                .HasIndex(s => s.Id)
                .IsUnique();

            modelBuilder.Entity<ApiResource>()
                .HasMany(r => r.Scopes)
                .WithMany(s => s.Resources)
                .UsingEntity<Dictionary<string, object>>(
                    "resource_scope_mappings",
                    j => j.HasOne<ApiScope>().WithMany().HasForeignKey("scope_id"),
                    j => j.HasOne<ApiResource>().WithMany().HasForeignKey("resource_id"),
                    j => j.ToTable("resource_scope_mappings", "auth") 
                );

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Resources)
                .WithMany(r => r.Clients)
                .UsingEntity<Dictionary<string, object>>(
                    "client_resource_mappings",
                    j => j.HasOne<ApiResource>().WithMany().HasForeignKey("resource_id"),
                    j => j.HasOne<Client>().WithMany().HasForeignKey("client_id"),
                    j => j.ToTable("client_resource_mappings", "auth") 
                );

            modelBuilder.Entity<ApiResource>()
                .HasMany(r => r.Clients)
                .WithMany(c => c.Resources)
                .UsingEntity<Dictionary<string, object>>(
                    "client_resource_mappings",
                    j => j.HasOne<Client>().WithMany().HasForeignKey("client_id"),
                    j => j.HasOne<ApiResource>().WithMany().HasForeignKey("resource_id"),
                    j => j.ToTable("client_resource_mappings", "auth")
                );
        }
    }
}
