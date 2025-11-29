using Orcamentaria.EquipmentService.Domain.Models;
using Orcamentaria.EquipmentService.Test.Contexts;
using Orcamentaria.EquipmentService.Test.Fixtures;
using Xunit;

namespace Orcamentaria.EquipmentService.Test.Repositories
{
    [Collection(nameof(EquipmentCollection))]
    public class EquipmentRepositoryTest : BasicRepositoryTest<Equipment, MySqlContextTest>
    {
        
        public EquipmentRepositoryTest(EquipmentFixture fixture) : base(fixture)
        {
        }
    }
}
