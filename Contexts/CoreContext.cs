using Common.Data;
using DatabaseService.Data.Models.Auth;
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
        public DbSet<ResourceScopeMapping> ResourceScopeMappings { get; set; }
        public DbSet<ClientResourceMapping> ClientResourceMappings { get; set; }
        public DbSet<ClientScopeMapping> ClientScopeMappings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<ApiScope>()
                .HasIndex(s => s.Name)
                .IsUnique();

            modelBuilder.Entity<ApiResource>()
                .HasIndex(s => s.Name)
                .IsUnique();

            modelBuilder.Entity<ResourceScopeMapping>()
                .HasKey(x => new { x.ResourceId, x.ScopeId });

            modelBuilder.Entity<ResourceScopeMapping>()
                .HasOne(rsm => rsm.Resource)
                .WithMany()
                .HasForeignKey(rsm => rsm.ResourceId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<ResourceScopeMapping>()
                .HasOne(rsm => rsm.Scope)
                .WithMany()
                .HasForeignKey(rsm => rsm.ScopeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClientResourceMapping>()
                .HasKey(x => new { x.ClientId, x.ResourceId });

            modelBuilder.Entity<ClientResourceMapping>()
                .HasOne(rsm => rsm.Resource)
                .WithMany()
                .HasForeignKey(rsm => rsm.ResourceId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<ClientResourceMapping>()
                .HasOne(rsm => rsm.Client)
                .WithMany(ar => ar.AllowedScopes)
                .HasForeignKey(rsm => rsm.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClientScopeMapping>()
                .HasKey(x => new { x.ClientId, x.ScopeId });

            modelBuilder.Entity<ClientScopeMapping>()
                .HasOne(rsm => rsm.Client)
                .WithMany(ar => ar.AllowedResources)
                .HasForeignKey(rsm => rsm.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<ClientScopeMapping>()
                .HasOne(rsm => rsm.Scope)
                .WithMany()
                .HasForeignKey(rsm => rsm.ScopeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
