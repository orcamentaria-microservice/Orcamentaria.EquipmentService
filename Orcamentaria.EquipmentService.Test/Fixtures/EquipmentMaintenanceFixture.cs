using Orcamentaria.EquipmentService.Domain.Models;
using Orcamentaria.Lib.Test.Fixtures;
using Xunit;

namespace Orcamentaria.EquipmentService.Test.Fixtures
{
    [CollectionDefinition(nameof(EquipmentMaintenanceCollection))]
    public class EquipmentMaintenanceCollection : ICollectionFixture<EquipmentMaintenanceFixture> { }


    public class EquipmentMaintenanceFixture : BaseFixture<EquipmentMaintenance>
    {

        override
        public EquipmentMaintenance CreateEntity(long id)
        {
            return new EquipmentMaintenance
            {
                Id = id,
                Description = Faker.Commerce.ProductDescription(),
                EquipmentId = 1,
                CompanyId = 1,
                CreatedAt = Faker.Date.Past(),
                CreatedBy = 1
            };
        }
    }
}
