using Microsoft.EntityFrameworkCore;

namespace DatabaseService.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApiScope> ApiScopes { get; set; }
        public DbSet<ApiResource> ApiResources { get; set; }
        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

    public class ApplicationUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }

    public class ApiScope
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ApiResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ApiScope> Scopes { get; set; }
    }

    public class Client
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public List<ApiScope> AllowedScopes { get; set; }
    }
}
