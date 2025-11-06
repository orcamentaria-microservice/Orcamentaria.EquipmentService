using Microsoft.EntityFrameworkCore;
using Orcamentaria.EquipamentService.Domain.Models;
using Orcamentaria.EquipamentService.Infrastructure.Configurations;

namespace Orcamentaria.EquipamentService.Infrastructure.Contexts
{
    public class MySqlContext : DbContext
    {
        public MySqlContext(DbContextOptions<MySqlContext> options)
        : base(options)
        {
        }

        public DbSet<Equipament> Equipaments { get; set; }
        public DbSet<EquipamentType> EquipamentTypes { get; set; }
        public DbSet<EquipamentMaintenance> EquipamentMaintenances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EquipamentConfiguration());
            modelBuilder.ApplyConfiguration(new EquipamentTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EquipamentMaintenanceConfiguration());
        }
    }
}
