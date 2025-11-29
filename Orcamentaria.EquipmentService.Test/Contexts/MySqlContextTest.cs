using Microsoft.EntityFrameworkCore;
using Orcamentaria.EquipmentService.Domain.Models;
using Orcamentaria.Lib.Test.Contexts;

namespace Orcamentaria.EquipmentService.Test.Contexts
{
    public class MySqlContextTest : DbContextTest
    {
        public MySqlContextTest(DbContextOptions<DbContextTest> opts) : base(opts) { }
        public DbSet<Equipment> Equipments { get; set; } = null!;
        public DbSet<EquipmentType> EquipmentTypes { get; set; } = null!;
        public DbSet<EquipmentMaintenance> EquipmentMaintenances { get; set; } = null!;
    }
}
