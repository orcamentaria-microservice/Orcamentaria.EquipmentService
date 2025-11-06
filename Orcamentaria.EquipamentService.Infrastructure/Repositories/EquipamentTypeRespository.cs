using Orcamentaria.EquipamentService.Domain.Models;
using Orcamentaria.EquipamentService.Domain.Repositories;
using Orcamentaria.EquipamentService.Infrastructure.Contexts;
using Orcamentaria.Lib.Domain.Contexts;
using Orcamentaria.Lib.Infrastructure.Repositories;

namespace Orcamentaria.EquipamentService.Infrastructure.Repositories
{
    public class EquipamentTypeRespository : BasicRepository<EquipamentType>, IEquipamentTypeRepository
    {
        public EquipamentTypeRespository(
            MySqlContext context, 
            IUserAuthContext userAuthContext) 
            : base(context, userAuthContext)
        {
        }
    }
}
