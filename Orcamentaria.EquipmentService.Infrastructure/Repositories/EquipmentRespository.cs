using Orcamentaria.EquipmentService.Domain.Models;
using Orcamentaria.EquipmentService.Domain.Repositories;
using Orcamentaria.EquipmentService.Infrastructure.Contexts;
using Orcamentaria.Lib.Domain.Contexts;
using Orcamentaria.Lib.Infrastructure.Repositories;

namespace Orcamentaria.EquipmentService.Infrastructure.Repositories
{
    public class EquipmentRespository : BaseRepository<Equipment>, IEquipmentRepository
    {
        public EquipmentRespository(
            MySqlContext context, 
            IUserAuthContext userAuthContext) 
            : base(context, userAuthContext)
        {
        }
    }
}
