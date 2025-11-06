using Orcamentaria.EquipamentService.Domain.Models;
using Orcamentaria.EquipamentService.Domain.Repositories;
using Orcamentaria.EquipamentService.Infrastructure.Contexts;
using Orcamentaria.Lib.Domain.Contexts;
using Orcamentaria.Lib.Infrastructure.Repositories;

namespace Orcamentaria.EquipamentService.Infrastructure.Repositories
{
    public class EquipamentRespository : BasicRepository<Equipament>, IEquipamentRepository
    {
        public EquipamentRespository(
            MySqlContext context, 
            IUserAuthContext userAuthContext) 
            : base(context, userAuthContext)
        {
        }
    }
}
