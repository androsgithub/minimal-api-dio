using asp_minimals_apis.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace asp_minimals_apis.Infrastructure.Db
{
    public class InfraDbContext : DbContext
    {
        private readonly IConfiguration _configAppSettings;
        public InfraDbContext(IConfiguration configAppSettings)
        {
            _configAppSettings = configAppSettings;
        }
        public DbSet<Admin> Admins { get; set; } = default!;
        public DbSet<Vehicle> Vehicles { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().HasData(new Admin
            {
                Id = 1,
                Email = "admin@test.com",
                Password = "12345",
                Profile = "Master"
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configAppSettings.GetConnectionString("MySql")?.ToString();
                if (!string.IsNullOrEmpty(connectionString))
                {
                    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                }
            }

        }

    }
}