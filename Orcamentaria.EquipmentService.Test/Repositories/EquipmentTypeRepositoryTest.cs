using Orcamentaria.EquipmentService.Domain.Models;
using Orcamentaria.EquipmentService.Test.Contexts;
using Orcamentaria.EquipmentService.Test.Fixtures;
using Orcamentaria.Lib.Test.Repositories;
using Xunit;

namespace Orcamentaria.EquipmentService.Test.Repositories
{
    [Collection(nameof(EquipmentTypeCollection))]
    public class EquipmentTypeReadRepositoryTest : ReadWithCompanyRepositoryTests<EquipmentType, MySqlContextTest>
    {
        public EquipmentTypeReadRepositoryTest(EquipmentTypeFixture fixture) : base(fixture) { }
    }

    [Collection(nameof(EquipmentTypeCollection))]
    public class EquipmentTypeWriteRepositoryTest : WriteWithCompanyRepositoryTests<EquipmentType, MySqlContextTest>
    {
        public EquipmentTypeWriteRepositoryTest(EquipmentTypeFixture fixture) : base(fixture) { }
    }

    [Collection(nameof(EquipmentTypeCollection))]
    public class EquipmentTypeDeleteRepositoryTest : DeleteWithCompanyRepositoryTests<EquipmentType, MySqlContextTest>
    {
        public EquipmentTypeDeleteRepositoryTest(EquipmentTypeFixture fixture) : base(fixture) { }
    }
}
