using Orcamentaria.EquipmentService.Domain.Models;
using Orcamentaria.Lib.Test.Fixtures;
using Xunit;

namespace Orcamentaria.EquipmentService.Test.Fixtures
{
    [CollectionDefinition(nameof(EquipmentTypeCollection))]
    public class EquipmentTypeCollection : ICollectionFixture<EquipmentTypeFixture> { }


    public class EquipmentTypeFixture : BaseFixture<EquipmentType>
    {

        override
        public EquipmentType CreateEntity(long id)
        {
            return new EquipmentType
            {
                Id = id,
                Name = Faker.Name.FirstName(),
                Active = true,
                CompanyId = 1,
                CreatedAt = Faker.Date.Past(),
                CreatedBy = 1,
                UpdatedAt = Faker.Date.Future(),
                UpdatedBy = 1
            };
        }
    }
}
