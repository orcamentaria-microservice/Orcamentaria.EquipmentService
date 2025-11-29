using Orcamentaria.EquipmentService.Domain.Models;

namespace Orcamentaria.EquipmentService.Domain.Services.Internal
{
    public interface IEquipmentTypeInternalService
    {
        Task<EquipmentType?> GetByIdAsync(long id);
    }
}
