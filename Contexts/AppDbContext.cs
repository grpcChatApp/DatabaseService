using Common.Data;
using DatabaseService.Data.Models;
using DatabaseService.Data.KafkaEvents;
using Microsoft.EntityFrameworkCore;

namespace DatabaseService.Contexts
{
    public class AppContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        // public DbSet<Role> Roles => Set<Role>();

        public AppContext(DbContextOptions<AppContext> options)
            : base(options) { }
    }
}