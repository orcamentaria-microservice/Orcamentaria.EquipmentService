using Orcamentaria.EquipmentService.Domain.Models;
using Orcamentaria.EquipmentService.Test.Contexts;
using Orcamentaria.EquipmentService.Test.Fixtures;
using Xunit;

namespace Orcamentaria.EquipmentService.Test.Repositories
{
    [Collection(nameof(EquipmentTypeCollection))]
    public class EquipmentTypeRepositoryTest : BasicRepositoryTest<EquipmentType, MySqlContextTest>
    {
        
        public EquipmentTypeRepositoryTest(EquipmentTypeFixture fixture) : base(fixture)
        {
        }
    }
}
