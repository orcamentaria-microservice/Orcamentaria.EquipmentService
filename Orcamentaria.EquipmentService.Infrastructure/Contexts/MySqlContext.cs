using Microsoft.EntityFrameworkCore;
using Orcamentaria.EquipmentService.Domain.Models;
using Orcamentaria.EquipmentService.Infrastructure.Configurations;

namespace Orcamentaria.EquipmentService.Infrastructure.Contexts
{
    public class MySqlContext : DbContext
    {
        public MySqlContext(DbContextOptions<MySqlContext> options)
        : base(options)
        {
        }

        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<EquipmentMaintenance> EquipmentMaintenances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EquipmentConfiguration());
            modelBuilder.ApplyConfiguration(new EquipmentTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EquipmentMaintenanceConfiguration());
        }
    }
}
