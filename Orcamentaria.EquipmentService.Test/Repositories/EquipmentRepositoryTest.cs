using Orcamentaria.EquipmentService.Domain.Models;
using Orcamentaria.EquipmentService.Test.Contexts;
using Orcamentaria.EquipmentService.Test.Fixtures;
using Orcamentaria.Lib.Test.Repositories;
using Xunit;

namespace Orcamentaria.EquipmentService.Test.Repositories
{
    [Collection(nameof(EquipmentCollection))]
    public class EquipmentReadRepositoryTest : ReadWithCompanyRepositoryTests<Equipment, MySqlContextTest>
    {
        public EquipmentReadRepositoryTest(EquipmentFixture fixture) : base(fixture) { }
    }

    [Collection(nameof(EquipmentCollection))]
    public class EquipmentWriteRepositoryTest : WriteWithCompanyRepositoryTests<Equipment, MySqlContextTest>
    {
        public EquipmentWriteRepositoryTest(EquipmentFixture fixture) : base(fixture) { }
    }
}
