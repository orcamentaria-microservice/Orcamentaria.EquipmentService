using Orcamentaria.EquipmentService.Domain.Models;
using Orcamentaria.EquipmentService.Test.Contexts;
using Orcamentaria.EquipmentService.Test.Fixtures;
using Xunit;

namespace Orcamentaria.EquipmentService.Test.Repositories
{
    [Collection(nameof(EquipmentMaintenanceCollection))]
    public class EquipmentMaintenanceRepositoryTest : BasicRepositoryTest<EquipmentMaintenance, MySqlContextTest>
    {
        
        public EquipmentMaintenanceRepositoryTest(EquipmentMaintenanceFixture fixture) : base(fixture)
        {
        }
    }
}
