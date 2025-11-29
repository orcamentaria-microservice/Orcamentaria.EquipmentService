using Orcamentaria.EquipmentService.Domain.Enums;
using Orcamentaria.EquipmentService.Domain.Models;
using Orcamentaria.Lib.Test.Fixtures;
using Xunit;

namespace Orcamentaria.EquipmentService.Test.Fixtures
{
    [CollectionDefinition(nameof(EquipmentCollection))]
    public class EquipmentCollection : ICollectionFixture<EquipmentFixture> { }


    public class EquipmentFixture : BaseFixture<Equipment>
    {

        override
        public Equipment CreateEntity(long id)
        {
            return new Equipment
            {
                Id = id,
                Name = Faker.Name.FirstName(),
                Description = Faker.Commerce.ProductDescription(),
                Active = true,
                Manufacturer = Faker.Name.LastName(),
                MaintenancePeriod = MaintenancePeriodEnum.Monthly,
                TypeId = 1,
                CompanyId = 1,
                CreatedAt = Faker.Date.Past(),
                CreatedBy = 1,
                UpdatedAt = Faker.Date.Future(),
                UpdatedBy = 1
            };
        }
    }
}
