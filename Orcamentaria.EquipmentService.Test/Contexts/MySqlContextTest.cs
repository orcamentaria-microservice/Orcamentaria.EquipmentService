using Microsoft.EntityFrameworkCore;
using Orcamentaria.EquipmentService.Infrastructure.Contexts;

namespace Orcamentaria.EquipmentService.Test.Contexts
{
    public class MySqlContextTest : MySqlContext
    {
        public MySqlContextTest(DbContextOptions<DbContext> options) : base(options) { }
    }
}
