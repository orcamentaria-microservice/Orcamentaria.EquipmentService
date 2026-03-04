using Orcamentaria.EquipmentService.Domain.Models;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Responses;
using Orcamentaria.Lib.Domain.Repositories;
using System.Linq.Expressions;

namespace Orcamentaria.EquipmentService.Domain.Repositories
{
    public interface IEquipmentMaintenanceRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(long id, params Expression<Func<TEntity, object>>[] includes);
        Task<(IEnumerable<TEntity?>, ResponsePagination pagination)> GetAsync(GridParams gridParams, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> InsertAsync(TEntity entity);
        Task<TEntity> UpdateAsync(long id, TEntity entity);
    }
}
