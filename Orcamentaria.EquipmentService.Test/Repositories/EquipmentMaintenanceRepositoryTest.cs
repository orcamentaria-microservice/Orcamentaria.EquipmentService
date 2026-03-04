using Orcamentaria.EquipmentService.Domain.Models;
using Orcamentaria.EquipmentService.Test.Contexts;
using Orcamentaria.EquipmentService.Test.Fixtures;
using Orcamentaria.Lib.Test.Repositories;
using Xunit;

namespace Orcamentaria.EquipmentService.Test.Repositories
{
    [Collection(nameof(EquipmentMaintenanceCollection))]
    public class EquipmentMaintenanceReadRepositoryTest : ReadWithCompanyRepositoryTests<EquipmentMaintenance, MySqlContextTest>
    {
        public EquipmentMaintenanceReadRepositoryTest(EquipmentMaintenanceFixture fixture) : base(fixture) { }
    }

    [Collection(nameof(EquipmentMaintenanceCollection))]
    public class EquipmentMaintenanceWriteRepositoryTest : WriteWithCompanyRepositoryTests<EquipmentMaintenance, MySqlContextTest>
    {
        public EquipmentMaintenanceWriteRepositoryTest(EquipmentMaintenanceFixture fixture) : base(fixture) { }
    }
}
