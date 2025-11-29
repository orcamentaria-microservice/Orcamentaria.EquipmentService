using Orcamentaria.EquipmentService.Domain.Models;

namespace Orcamentaria.EquipmentService.Domain.Services.Internal
{
    public interface IEquipmentInternalService
    {
        Task<Equipment?> GetByIdAsync(long id);
    }
}
