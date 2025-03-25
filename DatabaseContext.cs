using Microsoft.EntityFrameworkCore;
using Common;
using Common.Data;
using static Common.Constants;

namespace DatabaseService
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<ApiScope> ApiScopes { get; set; }
        public DbSet<ApiResource> ApiResources { get; set; }
        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
               .HasIndex(u => u.Username)
               .IsUnique();

            modelBuilder.Entity<ApiResource>()
                .HasMany(r => r.Scopes)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.AllowedScopes)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApiScope>().HasData(
                new ApiScope { Id = (int)PermissionsEnum.None, Name = Enum.GetName(PermissionsEnum.None)! },
                new ApiScope { Id = (int)PermissionsEnum.Read, Name = Enum.GetName(PermissionsEnum.Read)! },
                new ApiScope { Id = (int)PermissionsEnum.Write, Name = Enum.GetName(PermissionsEnum.Write)! }
            );

            modelBuilder.Entity<ApiResource>().HasData(
                new ApiResource { Id = 1, Name = ServiceNames.ClientApp },
                new ApiResource { Id = 2, Name = ServiceNames.BrokerService },
                new ApiResource { Id = 3, Name = ServiceNames.DatabaseService },
                new ApiResource { Id = 4, Name = ServiceNames.BackendHostService },
                new ApiResource { Id = 5, Name = ServiceNames.AuthenticationServer }
            );
        }
    }
}
